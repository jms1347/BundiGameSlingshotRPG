using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float speed = 2f;
    public Transform player;

    private float attackTimer;
    public float attackInterval = 5f;

    private void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        attackTimer -= Time.deltaTime;
        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackInterval;
        }

        if (directionToPlayer.magnitude > 0.1f)
        {
            transform.forward = directionToPlayer;
        }
    }

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

    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= attackRange;
    }

    private void AttackPlayer()
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        Vector3 knockbackDirection = (player.position - transform.position).normalized;
        playerRigidbody.AddForce(knockbackDirection * 15f, ForceMode.Impulse);
    }
}