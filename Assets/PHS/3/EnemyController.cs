using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement & Attack Settings")]
    public float attackRange = 1.5f;        // �÷��̾ ������ �� �ִ� ����
    public float speed = 2f;                // �÷��̾ �����ϴ� �ӵ�
    public float attackInterval = 5f;       // ���� ���� (��)
    public float knockbackForce = 15f;      // �÷��̾� ���� �� ������ �˹� ��

    [Header("References")]
    public Transform player;              // ������ �÷��̾� ������Ʈ

    private float attackTimer = 0f;       // ���� Ÿ�̸�

    void Update()
    {
        if (player == null)
            return;

        // �÷��̾� ���� �� �Ÿ� ���
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // �÷��̾ ���� ���� ���̸� �÷��̾ ���� �̵�
        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        // ���� Ÿ�̸� ���� ��, ���� ���� ���̸� ���� �����ϴ� ���� (���� �ּ� ó����)
        /*
        attackTimer -= Time.deltaTime;
        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackInterval;
        }
        */

        // ���� �׻� �÷��̾ �ٶ󺸵��� ȸ��
        if (directionToPlayer.magnitude > 0.1f)
        {
            transform.forward = directionToPlayer;
        }
    }

    // �÷��̾ �����ϴ� �޼��� (�ʿ信 ���� ���� ���� ���� �߰�)
    private void AttackPlayer()
    {
        // �Ʒ� ����� �α� �� ���� ������ ���� �ּ� ó���Ǿ� �ֽ��ϴ�.
        // Debug.Log("Enemy attacking player!");
        // ��: �÷��̾�� �������� �ִ� ������ �߰��� �� ����.
    }

    // �浹 �̺�Ʈ: �÷��̾� ���� ������Ʈ�� �浹 �� �˹� ȿ�� �����ϴ� ���� (���� �ּ� ó����)
    void OnCollisionEnter(Collision collision)
    {
        /*
        // ���� ���, �±׳� Ư�� ������Ʈ�� ���� ���� ������Ʈ�� �ĺ��ϴ� ������ ���� ���
        if (collision.gameObject.GetComponent<PlayerAttackMarker>() != null)
        {
            Debug.Log("Enemy hit by player attack!");

            Rigidbody enemyRb = GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                // ���� sleep ���¶�� ����
                enemyRb.WakeUp();

                // �浹�� ��ü�� ��ġ�� ���� ��ġ�� �������� �˹� ���� ���
                Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

                // �˹� �� ���� (ForceMode.Impulse: ��� ���� ����)
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                Debug.Log("Applied knockback force: " + knockbackForce);
            }
        }
        */
    }

    // �� ������ �־��� ��ġ�� ���� ����� ���� ��ȯ�ϴ� ��ƿ��Ƽ �Լ�
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

    // Ư�� ��ġ�� ���� ���� ���� ���� �ִ��� Ȯ���ϴ� �Լ�
    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= attackRange;
    }
}