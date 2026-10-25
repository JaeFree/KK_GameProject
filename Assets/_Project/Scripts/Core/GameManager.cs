using System;
using UnityEngine;

namespace KK.Core
{
    /// <summary>게임 전체의 진행 상태입니다.</summary>
    public enum GameState
    {
        Ready,      // 시작 전 (타이틀, 대기)
        Playing,    // 플레이 중
        Paused,     // 일시정지
        GameOver,   // 패배
        Clear,      // 승리
    }

    /// <summary>
    /// 게임 상태(시작, 일시정지, 종료)를 한곳에서 관리합니다.
    /// 씬에 하나만 두고, 다른 스크립트는 GameManager.Instance로 접근합니다.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState State { get; private set; } = GameState.Ready;

        /// <summary>상태가 바뀔 때 (이전 상태, 새 상태)로 알립니다. UI가 구독해서 화면을 바꿉니다.</summary>
        public event Action<GameState, GameState> StateChanged;

        [Tooltip("씬이 시작되면 바로 Playing 상태로 들어갈지 여부 (테스트용)")]
        [SerializeField] bool startOnAwake = true;

        void Awake()
        {
            // 같은 씬에 GameManager가 두 개 생기면 나중 것을 지웁니다.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            if (startOnAwake) StartGame();
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void StartGame() => ChangeState(GameState.Playing);

        public void TogglePause()
        {
            if (State == GameState.Playing) ChangeState(GameState.Paused);
            else if (State == GameState.Paused) ChangeState(GameState.Playing);
        }

        public void GameOver() => ChangeState(GameState.GameOver);

        public void Clear() => ChangeState(GameState.Clear);

        void ChangeState(GameState next)
        {
            if (State == next) return;

            GameState previous = State;
            State = next;

            // 일시정지일 때만 시간을 멈춥니다. 결과 화면 연출이 필요하면 여기서 바꿉니다.
            Time.timeScale = next == GameState.Paused ? 0f : 1f;

            StateChanged?.Invoke(previous, next);
        }
    }
}
