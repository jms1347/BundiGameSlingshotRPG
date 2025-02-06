using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float attackRange = 1.5f;        // 플레이어를 공격할 수 있는 범위 (이 범위 내면 공격을 진행할 수 있음)
    public float speed = 2f;                // 플레이어 추적 속도

    [Header("Knockback Settings")]
    public float knockbackForce = 15f;      // 기본 넉백 힘 (외부에서 전달받은 값 대신 기본값으로 사용할 수 있음)
    public float knockbackResistance = 1f;  // 적의 넉백 저항 (1 이상일 경우 넉백 효과가 약해짐)

    [Header("References")]
    public Transform player;              // 추적할 플레이어의 Transform

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 적의 회전이 과도하게 이루어지지 않도록 X, Z축 회전은 제한합니다.
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (player == null)
            return;

        // 플레이어와의 방향 및 거리 계산
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 플레이어가 공격 범위 밖이면 플레이어를 향해 이동(추적)
        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        // 적은 항상 플레이어를 바라보도록 회전
        if (directionToPlayer.magnitude > 0.1f)
        {
            transform.forward = directionToPlayer;
        }
    }

    /// <summary>
    /// 외부에서 호출하여 적에게 넉백 효과를 적용하는 메서드입니다.
    /// </summary>
    /// <param name="force">적용할 넉백 힘</param>
    /// <param name="sourcePosition">공격이 발생한 위치 (플레이어 위치 등)</param>
    public void ApplyKnockback(float force, Vector3 sourcePosition)
    {
        if (rb != null)
        {
            rb.WakeUp();

            // knockbackResistance가 0 이하일 경우를 대비해 최소 1로 설정
            float effectiveResistance = (knockbackResistance <= 0f) ? 1f : knockbackResistance;
            float effectiveForce = force / effectiveResistance;

            // 공격 원점(플레이어 위치)에서 멀어지는 방향으로 넉백을 적용합니다.
            Vector3 knockbackDir = (transform.position - sourcePosition).normalized;
            rb.AddForce(knockbackDir * effectiveForce, ForceMode.Impulse);

            Debug.Log("Applied knockback force: " + effectiveForce);
        }
    }

    /*
    // 만약 OnCollisionEnter()에서 플레이어 공격 오브젝트와의 충돌을 통해 넉백을 적용하고 싶다면,
    // 아래 코드를 사용하여 처리할 수 있습니다.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            if (rb != null)
            {
                rb.WakeUp();
                Vector3 knockbackDir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
                Debug.Log("Applied knockback force from collision: " + knockbackForce);
            }
        }
    }
    */

    /// <summary>
    /// 씬 내에서 주어진 위치에 가장 가까운 적을 반환하는 유틸리티 함수입니다.
    /// </summary>
    /// <param name="position">비교할 위치</param>
    /// <returns>가장 가까운 EnemyController 인스턴스</returns>
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

    /// <summary>
    /// 주어진 위치가 이 적의 공격 범위 내에 있는지 판단하는 함수입니다.
    /// </summary>
    /// <param name="position">비교할 위치</param>
    /// <returns>공격 범위 내이면 true, 그렇지 않으면 false</returns>
    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= attackRange;
    }
}