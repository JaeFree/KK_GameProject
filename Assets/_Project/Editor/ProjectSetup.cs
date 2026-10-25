using System.Collections.Generic;
using System.IO;
using System.Linq;
using KK.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace KK.EditorTools
{
    /// <summary>
    /// 메뉴 KK > 프로젝트 기본 설정 적용 한 번으로 협업에 필요한 설정과 기본 씬을 만듭니다.
    /// 여러 번 실행해도 이미 있는 씬은 덮어쓰지 않습니다.
    /// </summary>
    public static class ProjectSetup
    {
        const string ScenesFolder = "Assets/_Project/Scenes";
        const string MainScenePath = ScenesFolder + "/Main.unity";
        const string SandboxScenePath = ScenesFolder + "/Sandbox_JaeFree.unity";

        static readonly string[] Folders =
        {
            "Assets/_Project/Scripts/Core",
            "Assets/_Project/Scripts/Combat",
            "Assets/_Project/Scripts/Player",
            "Assets/_Project/Scripts/Enemy",
            "Assets/_Project/Scripts/Systems",
            "Assets/_Project/Scripts/UI",
            "Assets/_Project/Data",
            "Assets/_Project/Prefabs",
            ScenesFolder,
            "Assets/_Project/Art/Materials",
            "Assets/_Project/Art/Models",
            "Assets/_Project/Audio/BGM",
            "Assets/_Project/Audio/SFX",
            "Assets/ThirdParty",
        };

        [MenuItem("KK/프로젝트 기본 설정 적용")]
        public static void Apply()
        {
            // 작업 중인 씬이 있으면 먼저 저장할지 묻습니다. 취소하면 아무것도 하지 않습니다.
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            var log = new List<string>();

            ApplyEditorSettings(log);
            CreateFolders(log);
            CreateSceneIfMissing(MainScenePath, withGameManager: true, log);
            CreateSceneIfMissing(SandboxScenePath, withGameManager: true, log);
            AddToBuildSettings(MainScenePath, log);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene(MainScenePath);

            string summary = log.Count > 0 ? string.Join("\n", log) : "이미 모두 설정돼 있습니다.";
            Debug.Log("[프로젝트 설정]\n" + summary);
            EditorUtility.DisplayDialog("프로젝트 기본 설정 완료", summary, "확인");
        }

        static void ApplyEditorSettings(List<string> log)
        {
            // Git으로 씬/프리팹 변경 내용을 비교하고 병합하려면 텍스트 저장이 필요합니다.
            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                EditorSettings.serializationMode = SerializationMode.ForceText;
                log.Add("- Asset Serialization: Force Text");
            }

            // .meta 파일이 보여야 Git에 같이 올라갑니다.
            if (VersionControlSettings.mode != "Visible Meta Files")
            {
                VersionControlSettings.mode = "Visible Meta Files";
                log.Add("- Version Control: Visible Meta Files");
            }

            if (PlayerSettings.companyName != "KK")
            {
                PlayerSettings.companyName = "KK";
                PlayerSettings.productName = "KK_GameProject";
                log.Add("- Company / Product Name: KK / KK_GameProject");
            }
        }

        static void CreateFolders(List<string> log)
        {
            foreach (string folder in Folders)
            {
                if (AssetDatabase.IsValidFolder(folder)) continue;

                string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
                AssetDatabase.CreateFolder(parent, Path.GetFileName(folder));
                log.Add("- 폴더 생성: " + folder);
            }
        }

        static void CreateSceneIfMissing(string path, bool withGameManager, List<string> log)
        {
            if (File.Exists(path)) return;

            // 기본 카메라와 조명이 들어 있는 새 씬
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // 탑다운 시점으로 내려다보는 카메라
            var camera = Camera.main;
            if (camera != null)
            {
                camera.transform.SetPositionAndRotation(new Vector3(0f, 15f, -10f), Quaternion.Euler(55f, 0f, 0f));
            }

            // 30 x 30 크기의 바닥
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(3f, 1f, 3f);

            if (withGameManager)
            {
                new GameObject("GameManager").AddComponent<GameManager>();
            }

            EditorSceneManager.SaveScene(scene, path);
            log.Add("- 씬 생성: " + path);
        }

        static void AddToBuildSettings(string path, List<string> log)
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == path)) return;

            // 게임 시작 씬이 맨 앞에 오도록 넣습니다.
            scenes.Insert(0, new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            log.Add("- 빌드 씬 목록에 추가: " + path);
        }
    }
}
