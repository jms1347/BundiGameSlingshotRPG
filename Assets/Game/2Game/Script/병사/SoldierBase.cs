using UnityEngine;
using System.Collections; // Coroutine을 사용하기 위함

public abstract class SoldierBase : MonoBehaviour
{
    // === 공통 데이터베이스 ===
    [Header("Base Stats")]
    public float moveSpeed = 3f;           // 이동 속도
    public float searchRadius = 5f;        // 적 탐지 반경
    public float attackSpeed = 1.5f;       // 공격 쿨타임 (초)
    public int attackDamage = 10;          // 공격력

    [Header("Internal State")]
    public float currentHealth = 100f;     // 현재 체력
    public string enemyTag = "Enemy";      // 적을 식별할 태그
    public Transform targetEnemy;          // 현재 타겟으로 지정된 적
    public float lastAttackTime;          // 마지막 공격 시간
    private SpriteRenderer spriteRenderer; // 시각적 피드백을 위한 컴포넌트

    // === 상태 변수 ===
    protected bool isAttacking = false;
    protected bool isMoving = false;

    // 초기화
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this object.");
        }
    }

    // 매 프레임마다 실행
    protected virtual void Update()
    {
        // 1. 적 탐색
        if (targetEnemy == null)
        {
            FindNearestEnemy();
        }

        // 2. 적이 있으면 행동
        if (targetEnemy != null)
        {
            // 2-1. 적에게 접근
            float distanceToTarget = Vector2.Distance(transform.position, targetEnemy.position);
            if (distanceToTarget > 0.1f) // 0.1f는 유닛 충돌을 방지하기 위한 여유값
            {
                MoveToTarget();
            }

            // 2-2. 적에게 닿았을 때 공격
            if (distanceToTarget <= 0.1f && Time.time >= lastAttackTime + attackSpeed)
            {
                AttackTarget();
            }
        }
    }

    // === 공통 함수 ===
    protected virtual void FindNearestEnemy()
    {
        // 주변의 모든 콜라이더를 탐지
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

        // 공격 로직. (예: 데미지 주기)
        var enemyHealthComponent = targetEnemy.GetComponent<SoldierBase>(); // 적도 이 클래스를 상속받는다고 가정
        if (enemyHealthComponent != null)
        {
            enemyHealthComponent.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {targetEnemy.name} for {attackDamage} damage.");
        }

        lastAttackTime = Time.time;
        isAttacking = true;
    }

    // 피해를 입었을 때
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

    // 사망 처리
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // 유닛 파괴
    }

    // === 시각적 피드백: 빨갛게 변했다 돌아오기 ===
    protected IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return Utils.WaitForSecond(0.1f); // 0.1초 동안 빨간색 유지
            spriteRenderer.color = originalColor;
        }
    }
}