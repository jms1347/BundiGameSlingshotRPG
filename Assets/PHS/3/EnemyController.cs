using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float attackRange = 1.5f;        // �÷��̾ ������ �� �ִ� ���� (�� ���� ���� ������ ������ �� ����)
    public float speed = 2f;                // �÷��̾� ���� �ӵ�

    [Header("Knockback Settings")]
    public float knockbackForce = 15f;      // �⺻ �˹� �� (�ܺο��� ���޹��� �� ��� �⺻������ ����� �� ����)
    public float knockbackResistance = 1f;  // ���� �˹� ���� (1 �̻��� ��� �˹� ȿ���� ������)

    [Header("References")]
    public Transform player;              // ������ �÷��̾��� Transform

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ���� ȸ���� �����ϰ� �̷������ �ʵ��� X, Z�� ȸ���� �����մϴ�.
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (player == null)
            return;

        // �÷��̾���� ���� �� �Ÿ� ���
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // �÷��̾ ���� ���� ���̸� �÷��̾ ���� �̵�(����)
        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        // ���� �׻� �÷��̾ �ٶ󺸵��� ȸ��
        if (directionToPlayer.magnitude > 0.1f)
        {
            transform.forward = directionToPlayer;
        }
    }

    /// <summary>
    /// �ܺο��� ȣ���Ͽ� ������ �˹� ȿ���� �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="force">������ �˹� ��</param>
    /// <param name="sourcePosition">������ �߻��� ��ġ (�÷��̾� ��ġ ��)</param>
    public void ApplyKnockback(float force, Vector3 sourcePosition)
    {
        if (rb != null)
        {
            rb.WakeUp();

            // knockbackResistance�� 0 ������ ��츦 ����� �ּ� 1�� ����
            float effectiveResistance = (knockbackResistance <= 0f) ? 1f : knockbackResistance;
            float effectiveForce = force / effectiveResistance;

            // ���� ����(�÷��̾� ��ġ)���� �־����� �������� �˹��� �����մϴ�.
            Vector3 knockbackDir = (transform.position - sourcePosition).normalized;
            rb.AddForce(knockbackDir * effectiveForce, ForceMode.Impulse);

            Debug.Log("Applied knockback force: " + effectiveForce);
        }
    }

    /*
    // ���� OnCollisionEnter()���� �÷��̾� ���� ������Ʈ���� �浹�� ���� �˹��� �����ϰ� �ʹٸ�,
    // �Ʒ� �ڵ带 ����Ͽ� ó���� �� �ֽ��ϴ�.
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
    /// �� ������ �־��� ��ġ�� ���� ����� ���� ��ȯ�ϴ� ��ƿ��Ƽ �Լ��Դϴ�.
    /// </summary>
    /// <param name="position">���� ��ġ</param>
    /// <returns>���� ����� EnemyController �ν��Ͻ�</returns>
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
    /// �־��� ��ġ�� �� ���� ���� ���� ���� �ִ��� �Ǵ��ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="position">���� ��ġ</param>
    /// <returns>���� ���� ���̸� true, �׷��� ������ false</returns>
    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= attackRange;
    }
}