using UnityEngine;

public class DefaultAttackBehavior : SpecialAttackBehavior
{
    public override void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force)
    {
        // 기본 특수 공격: 예를 들어, 기존 슬링샷 이동과 유사하게 플레이어에게 힘을 가함
        Rigidbody rb = origin != null ? null : null; // 실제로는 플레이어의 Rigidbody를 참조해야 함.
        // 여기서는 간단히 Debug.Log로 표시합니다.
        Debug.Log("Default Attack executed. Force: " + force);
        // 필요에 따라 추가 효과(이펙트 등)를 구현하세요.
    }
}