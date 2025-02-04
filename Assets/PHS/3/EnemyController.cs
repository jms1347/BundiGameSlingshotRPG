using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement & Attack Settings")]
    public float attackRange = 1.5f;        // 플레이어를 공격할 수 있는 범위
    public float speed = 2f;                // 플레이어를 추적하는 속도
    public float attackInterval = 5f;       // 공격 간격 (초)
    public float knockbackForce = 15f;      // 플레이어 공격 시 적용할 넉백 힘

    [Header("References")]
    public Transform player;              // 추적할 플레이어 오브젝트

    private float attackTimer = 0f;       // 공격 타이머

    void Update()
    {
        if (player == null)
            return;

        // 플레이어 방향 및 거리 계산
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 플레이어가 공격 범위 밖이면 플레이어를 향해 이동
        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        // 공격 타이머 감소 후, 공격 범위 내이면 공격 실행하는 로직 (현재 주석 처리됨)
        /*
        attackTimer -= Time.deltaTime;
        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackInterval;
        }
        */

        // 적이 항상 플레이어를 바라보도록 회전
        if (directionToPlayer.magnitude > 0.1f)
        {
            transform.forward = directionToPlayer;
        }
    }

    // 플레이어를 공격하는 메서드 (필요에 따라 실제 공격 로직 추가)
    private void AttackPlayer()
    {
        // 아래 디버그 로그 및 공격 로직은 현재 주석 처리되어 있습니다.
        // Debug.Log("Enemy attacking player!");
        // 예: 플레이어에게 데미지를 주는 로직을 추가할 수 있음.
    }

    // 충돌 이벤트: 플레이어 공격 오브젝트와 충돌 시 넉백 효과 적용하는 로직 (현재 주석 처리됨)
    void OnCollisionEnter(Collision collision)
    {
        /*
        // 예를 들어, 태그나 특정 컴포넌트를 통해 공격 오브젝트를 식별하는 로직이 있을 경우
        if (collision.gameObject.GetComponent<PlayerAttackMarker>() != null)
        {
            Debug.Log("Enemy hit by player attack!");

            Rigidbody enemyRb = GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                // 적이 sleep 상태라면 깨움
                enemyRb.WakeUp();

                // 충돌한 객체의 위치와 적의 위치를 기준으로 넉백 방향 계산
                Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

                // 넉백 힘 적용 (ForceMode.Impulse: 즉시 힘을 가함)
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                Debug.Log("Applied knockback force: " + knockbackForce);
            }
        }
        */
    }

    // 씬 내에서 주어진 위치에 가장 가까운 적을 반환하는 유틸리티 함수
    public static EnemyController GetClosestEnemy(Vector3 position)
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        EnemyController closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    // 특정 위치가 적의 공격 범위 내에 있는지 확인하는 함수
    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= attackRange;
    }
}