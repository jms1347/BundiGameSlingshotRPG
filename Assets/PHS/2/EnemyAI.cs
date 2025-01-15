using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float attackRange = 1.5f;
    public float attackForce = 15f;
    public float attackInterval = 5f;

    private float attackTimer;

    void Update()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer > attackRange)
        {
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }

        transform.forward = directionToPlayer;

        attackTimer -= Time.deltaTime;
        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackInterval;
        }
    }

    private void AttackPlayer()
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        Vector3 knockbackDirection = (player.position - transform.position).normalized;
        playerRigidbody.AddForce(-knockbackDirection * attackForce, ForceMode.Impulse);
    }
}