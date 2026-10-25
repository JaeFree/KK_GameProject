namespace KK.Combat
{
    /// <summary>
    /// 데미지를 받을 수 있는 모든 것(플레이어, 적, 부서지는 상자 등)이 구현합니다.
    /// 공격하는 쪽은 상대가 무엇인지 몰라도 TryGetComponent&lt;IDamageable&gt;로 데미지를 줄 수 있습니다.
    /// </summary>
    public interface IDamageable
    {
        bool IsDead { get; }

        void TakeDamage(int amount);
    }
}
