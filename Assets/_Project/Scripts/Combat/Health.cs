using System;
using KK.Core;
using UnityEngine;

namespace KK.Combat
{
    /// <summary>
    /// 체력을 가진 오브젝트에 붙입니다. 플레이어와 적이 같이 씁니다.
    /// 사용 예:
    ///   if (other.TryGetComponent(out IDamageable target)) target.TakeDamage(10);
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1)] int maxHealth = 100;

        [Tooltip("체력이 0이 되면 오브젝트를 자동으로 비활성화할지 여부")]
        [SerializeField] bool disableOnDeath = true;

        public int Current { get; private set; }
        public int Max => maxHealth;
        public bool IsDead => Current <= 0;

        /// <summary>체력이 바뀔 때 (현재, 최대)로 알립니다. 체력바 UI가 구독합니다.</summary>
        public event Action<int, int> Changed;

        /// <summary>죽는 순간 한 번 알립니다.</summary>
        public event Action Died;

        void Awake()
        {
            Current = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead || amount <= 0) return;

            Current = Mathf.Max(Current - amount, 0);
            Changed?.Invoke(Current, maxHealth);
            GameEvents.RaiseDamaged(gameObject, amount);

            if (IsDead) Die();
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0) return;

            Current = Mathf.Min(Current + amount, maxHealth);
            Changed?.Invoke(Current, maxHealth);
        }

        /// <summary>부활이나 오브젝트 풀 재사용 시 체력을 가득 채웁니다.</summary>
        public void ResetHealth()
        {
            Current = maxHealth;
            Changed?.Invoke(Current, maxHealth);
        }

        void Die()
        {
            Died?.Invoke();
            GameEvents.RaiseDied(gameObject);

            if (disableOnDeath) gameObject.SetActive(false);
        }
    }
}
