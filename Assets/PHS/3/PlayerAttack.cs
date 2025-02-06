using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Stats")]
    public int attackPower = 10;             // 플레이어의 공격력
    public float knockbackPower = 1.0f;        // 플레이어의 넉백 강화 수치
    public float baseKnockbackForce = 10f;     // 기본 넉백 힘

    // 공격이 적에게 적중했을 때 호출되는 메서드 (예: 애니메이션 이벤트나 충돌 이벤트 등에서 호출)
    public void ApplyAttack(GameObject enemy)
    {
        // 적의 EnemyController 컴포넌트를 가져옵니다.
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            // 적의 넉백 저항(방어력과 비슷한 개념)이 0 이하일 경우 1로 처리하여 0으로 나누는 오류를 방지합니다.
            float enemyResistance = enemyController.knockbackResistance;
            if (enemyResistance <= 0)
                enemyResistance = 1f;

            // 플레이어의 공격력과 knockbackPower를 곱한 값에, 기본 넉백 힘을 곱하고, 적의 저항으로 나누어 최종 넉백 힘을 계산합니다.
            float effectiveKnockback = baseKnockbackForce * (knockbackPower * attackPower / enemyResistance);
            Debug.Log("Effective Knockback Force: " + effectiveKnockback);

            // 적에게 넉백을 적용합니다.
            enemyController.ApplyKnockback(effectiveKnockback, transform.position);
        }
        else
        {
            Debug.LogWarning("적 객체에 EnemyController 컴포넌트가 없습니다.");
        }
    }
}