using UnityEngine;
using UnityEngine.SceneManagement;

namespace KK.Systems
{
    /// <summary>
    /// 씬 이동을 한곳에서 처리합니다. 버튼의 OnClick에 연결해서 쓸 수도 있습니다.
    /// 이동할 씬은 File > Build Profiles(Build Settings)의 Scene List에 들어 있어야 합니다.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public const string MainScene = "Main";

        public static void Load(string sceneName)
        {
            // 일시정지 상태에서 넘어가도 새 씬이 멈춰 있지 않게 합니다.
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }

        public static void ReloadCurrent() => Load(SceneManager.GetActiveScene().name);

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // 인스펙터의 버튼 OnClick에서 고를 수 있게 static이 아닌 버전도 둡니다.
        public void LoadScene(string sceneName) => Load(sceneName);
        public void ReloadScene() => ReloadCurrent();
        public void QuitGame() => Quit();
    }
}
