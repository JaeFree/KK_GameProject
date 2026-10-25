using System;
using UnityEngine;

namespace KK.Core
{
    /// <summary>
    /// 서로 모르는 스크립트끼리 소식을 주고받는 곳입니다.
    /// 예: 적이 죽으면 Enemy는 GameEvents.RaiseEnemyKilled만 부르고,
    ///     점수 UI와 경험치 시스템은 EnemyKilled를 구독해서 각자 처리합니다.
    /// 구독(+=)한 스크립트는 OnDisable에서 꼭 해제(-=)하세요.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>무언가 죽었을 때. 인자는 죽은 오브젝트입니다.</summary>
        public static event Action<GameObject> Died;

        /// <summary>데미지를 받았을 때. (맞은 오브젝트, 데미지 양)</summary>
        public static event Action<GameObject, int> Damaged;

        public static void RaiseDied(GameObject target) => Died?.Invoke(target);

        public static void RaiseDamaged(GameObject target, int amount) => Damaged?.Invoke(target, amount);

        // 에디터에서 Domain Reload를 끄고 플레이할 때 이전 구독이 남지 않게 초기화합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetOnPlay()
        {
            Died = null;
            Damaged = null;
        }
    }
}
