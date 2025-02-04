using UnityEngine;

// 특수 공격 모듈 인터페이스
public interface IAttackBehavior
{
    /// <summary>
    /// 특수 공격 실행.
    /// </summary>
    /// <param name="origin">공격 시작 위치</param>
    /// <param name="direction">공격 방향</param>
    /// <param name="force">공격에 적용할 힘</param>
    void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force);
}

// MonoBehaviour를 상속하여 Inspector에서 할당 가능하도록 추상 클래스 생성
public abstract class SpecialAttackBehavior : MonoBehaviour, IAttackBehavior
{
    public abstract void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force);
}