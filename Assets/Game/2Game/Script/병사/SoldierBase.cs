using UnityEngine;
using System.Collections; // Coroutine�� ����ϱ� ����

public abstract class SoldierBase : MonoBehaviour
{
    // === ���� �����ͺ��̽� ===
    [Header("Base Stats")]
    public float moveSpeed = 3f;           // �̵� �ӵ�
    public float searchRadius = 5f;        // �� Ž�� �ݰ�
    public float attackSpeed = 1.5f;       // ���� ��Ÿ�� (��)
    public int attackDamage = 10;          // ���ݷ�

    [Header("Internal State")]
    public float currentHealth = 100f;     // ���� ü��
    public string enemyTag = "Enemy";      // ���� �ĺ��� �±�
    public Transform targetEnemy;          // ���� Ÿ������ ������ ��
    public float lastAttackTime;          // ������ ���� �ð�
    private SpriteRenderer spriteRenderer; // �ð��� �ǵ���� ���� ������Ʈ

    // === ���� ���� ===
    protected bool isAttacking = false;
    protected bool isMoving = false;

    // �ʱ�ȭ
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this object.");
        }
    }

    // �� �����Ӹ��� ����
    protected virtual void Update()
    {
        // 1. �� Ž��
        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }

        // 2. ���� ������ �ൿ
        if (targetEnemy != null)
        {
            // 2-1. ������ ����
            float distanceToTarget = Vector2.Distance(transform.position, targetEnemy.position);
            if (distanceToTarget > 0.1f) // 0.1f�� ���� �浹�� �����ϱ� ���� ������
            {
                MoveToTarget();
            }

            // 2-2. ������ ����� �� ����
            if (distanceToTarget <= 0.1f && Time.time >= lastAttackTime + attackSpeed)
            {
                AttackTarget();
            }
        }
    }

    // === ���� �Լ� ===
    protected virtual void FindNearestEnemy()
    {
        // �ֺ��� ��� �ݶ��̴��� Ž��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        float closestDistance = Mathf.Infinity;
        Transform newTarget = null;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag(enemyTag))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    newTarget = hit.transform;
                }
            }
        }

        if (newTarget != null)
        {
            targetEnemy = newTarget;
            isMoving = true;
            Debug.Log($"Target found: {targetEnemy.name}");
        }
    }

    protected virtual void MoveToTarget()
    {
        if (targetEnemy == null) return;

        Vector2 direction = (targetEnemy.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    protected virtual void AttackTarget()
    {
        if (targetEnemy == null) return;

        // ���� ����. (��: ������ �ֱ�)
        var enemyHealthComponent = targetEnemy.GetComponent<SoldierBase>(); // ���� �� Ŭ������ ��ӹ޴´ٰ� ����
        if (enemyHealthComponent != null)
        {
            enemyHealthComponent.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {targetEnemy.name} for {attackDamage} damage.");
        }

        lastAttackTime = Time.time;
        isAttacking = true;
    }

    // ���ظ� �Ծ��� ��
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ��� ó��
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // ���� �ı�
    }

    // === �ð��� �ǵ��: ������ ���ߴ� ���ƿ��� ===
    protected IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return Utils.WaitForSecond(0.1f); // 0.1�� ���� ������ ����
            spriteRenderer.color = originalColor;
        }
    }
}