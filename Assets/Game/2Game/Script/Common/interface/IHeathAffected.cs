// 스킬의 영향을 받을 수 있는 모든 개체가 구현할 인터페이스 (선택 사항이지만 강력히 권장)
public interface IHealthAffected
{
    void TakeDamage(float amount);
    void TakeHeal(float amount);
    // bool IsAlive { get; }
}
