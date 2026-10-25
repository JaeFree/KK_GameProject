using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KK.EditorTools
{
    /// <summary>
    /// Unity 에디터를 열 때 한 번 Git 작업 안내 창을 띄웁니다.
    /// - 현재 브랜치와 원격보다 뒤처진 커밋 수를 보여 주고 pull을 안내합니다.
    /// - main 브랜치에 있으면 개별 브랜치를 만들도록 경고합니다.
    /// - 저장소의 .githooks 폴더를 git hook 경로로 자동 등록합니다.
    /// 메뉴 KK > Git 작업 안내 보기 로 언제든 다시 열 수 있습니다.
    /// </summary>
    [InitializeOnLoad]
    public static class GitWorkflowReminder
    {
        const string ShownKey = "KK_GitWorkflowReminder_Shown";
        const string HooksPath = ".githooks";

        static GitWorkflowReminder()
        {
            // 스크립트 컴파일마다 뜨지 않도록 에디터 실행당 한 번만 띄웁니다.
            if (SessionState.GetBool(ShownKey, false)) return;
            SessionState.SetBool(ShownKey, true);
            EditorApplication.delayCall += Show;
        }

        [MenuItem("KK/Git 작업 안내 보기")]
        public static void Show()
        {
            string branch = RunGit("rev-parse --abbrev-ref HEAD");
            if (string.IsNullOrEmpty(branch))
            {
                UnityEngine.Debug.LogWarning("[Git 안내] git을 찾지 못했습니다. Git for Windows가 설치돼 있는지 확인하세요.");
                return;
            }

            EnsureHooksPath();

            // 원격 상태를 가볍게 갱신한 뒤 뒤처진 커밋 수를 셉니다. 실패해도 안내는 계속합니다.
            RunGit("fetch --quiet origin", 8000);
            string behindText = RunGit("rev-list --count HEAD..origin/main");
            int.TryParse(behindText, out int behind);

            bool onMain = branch == "main" || branch == "master";

            string message = $"현재 브랜치: {branch}\n\n";
            if (behind > 0)
                message += $"원격 main에 아직 받지 않은 커밋이 {behind}개 있습니다. 먼저 pull 하세요.\n\n";

            if (onMain)
            {
                message +=
                    "main 브랜치에서는 직접 작업하지 마세요.\n\n" +
                    "작업 시작 순서\n" +
                    "1. git pull\n" +
                    "2. git switch -c feature/작업이름\n" +
                    "3. 작업 후 push 하고 GitHub에서 PR 만들기";
            }
            else
            {
                message +=
                    "작업 전에 main의 최신 내용을 받아 오세요.\n\n" +
                    "git switch main\n" +
                    "git pull\n" +
                    $"git switch {branch}\n" +
                    "git merge main";
            }

            EditorUtility.DisplayDialog(
                onMain ? "작업 전 확인: 개별 브랜치 + pull" : "작업 전 확인: pull",
                message,
                "확인");
        }

        static void EnsureHooksPath()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            if (!Directory.Exists(Path.Combine(projectRoot, HooksPath))) return;

            if (RunGit("config core.hooksPath") != HooksPath)
            {
                RunGit($"config core.hooksPath {HooksPath}");
                UnityEngine.Debug.Log("[Git 안내] git hook을 등록했습니다. 이제 main에 직접 커밋하면 차단됩니다.");
            }
        }

        static string RunGit(string args, int timeoutMs = 3000)
        {
            try
            {
                var info = new ProcessStartInfo("git", args)
                {
                    WorkingDirectory = Directory.GetParent(Application.dataPath).FullName,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                // 로그인 창 등으로 에디터가 멈추지 않게 합니다.
                info.EnvironmentVariables["GIT_TERMINAL_PROMPT"] = "0";
                info.EnvironmentVariables["GCM_INTERACTIVE"] = "never";

                using (var process = Process.Start(info))
                {
                    if (process == null) return null;
                    // 출력이 몇 줄뿐이라 먼저 기다렸다가 읽어도 막히지 않습니다.
                    if (!process.WaitForExit(timeoutMs))
                    {
                        process.Kill();
                        return null;
                    }
                    string output = process.StandardOutput.ReadToEnd();
                    return process.ExitCode == 0 ? output.Trim() : null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
