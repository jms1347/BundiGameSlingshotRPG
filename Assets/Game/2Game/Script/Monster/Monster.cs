using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster Attributes")]
    public float moveSpeed = 3f;
    public float chaseRange = 15f;
    public float attackRange = 1f;
    //public float rotationSpeed = 10f;

    public float attackDamage = 10f; // 몬스터가 주는 대미지 추가
    public float attackDuration = 0.2f; // 공격애니메이션 시간
    public float attackCooldown = 3f; // 공격속도

    [Header("Health")] // HP 시스템 추가
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100f; // 기본 최대 체력


    [Header("References")]
    public Transform playerTarget; // Assign your Player GameObject here in the Inspector
    [SerializeField] private Animator monsterAni;      // Assign the Monster's Animator component here

    private Vector3 initialPosition; // For potential idle wandering or returning home

    [Header("몬스터 상태")]
    public MonsterStateManager monsterStateManager;

    public Animator MonsterAni { get => monsterAni; set => monsterAni = value; }
    // HP 관련 프로퍼티
    public float CurrentHP
    {
        get => currentHP;
        private set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP); // HP가 0 미만, maxHP 초과하지 않도록 보정
            if (currentHP <= 0 && !IsDead()) // 죽음 상태 진입
            {
                monsterStateManager.ChangeState(MonsterState.DEATH);
            }
        }
    }
    public float MaxHP => maxHP;


    void Awake()
    {
        // 몬스터 상태 매니저가 유효한지 확인하고 설정
        if (monsterStateManager == null)
        {
            monsterStateManager = GetComponent<MonsterStateManager>();
            if (monsterStateManager == null)
            {
                Debug.LogError("MonsterStateManager not found on the same GameObject as Monster!", this);
                enabled = false; // 스크립트 비활성화
                return;
            }
        }
        monsterStateManager.SetMonster(this);

        initialPosition = transform.position; // Store initial position
        if (MonsterAni == null)
        {
            MonsterAni = GetComponent<Animator>();
        }

        CurrentHP = maxHP; // 게임 시작 시 HP를 최대로 설정
        // 게임 시작 시 타겟이 할당되지 않았다면 Player 태그를 가진 오브젝트를 찾아 할당
        if (playerTarget == null)
        {
            FindPlayerTarget();
        }
    }

    public void FindPlayerTarget()
    {
        // GameObject.FindWithTag는 비용이 높은 작업이므로 자주 호출하지 않는 것이 좋습니다.
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            Debug.Log($"Monster {gameObject.name}: Found Player target: {playerTarget.name}");
        }
        else
        {
            Debug.LogWarning($"Monster {gameObject.name}: Could not find GameObject with 'Player' tag.", this);
        }
    }

    public void MoveTowards(Vector3 targetPosition)
    {
        // 1. 타겟 방향 계산
        Vector3 direction = (targetPosition - transform.position).normalized;

        //// 2. Y축 기준으로만 회전하도록 설정 (몬스터가 눕거나 숙이지 않게)
        //if (direction != Vector3.zero) // 0 벡터일 때 쳐다볼 방향이 없으므로 회전하지 않도록 방지
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    // Slerp를 사용하여 부드럽게 회전
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        //if (MonsterAni != null) MonsterAni.SetBool("IsMoving", true);
    }

    public void StopMoving()
    {
        if (MonsterAni != null) MonsterAni.SetBool("IsMoving", false);
    }

    public void PerformAttack()
    {        
        // 1. 타겟 방향 계산
        Vector3 direction = (playerTarget.transform.position - transform.position).normalized;

        //// 2. Y축 기준으로만 회전하도록 설정 (몬스터가 눕거나 숙이지 않게)
        //if (direction != Vector3.zero) // 0 벡터일 때 쳐다볼 방향이 없으므로 회전하지 않도록 방지
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    // Slerp를 사용하여 부드럽게 회전
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}
        Debug.Log("Monster performs an attack!");
        // Add actual damage dealing logic here
    }

    // --- Conditions States Can Check ---

    public bool HasTarget()
    {
        return playerTarget != null;
    }

    public bool IsTargetInChaseRange()
    {
        if (playerTarget == null) return false;
        return Vector3.Distance(transform.position, playerTarget.position) <= chaseRange;
    }

    // --- 수정된 IsTargetInAttackRange 메서드 ---
    public bool IsTargetInAttackRange()
    {
        if (playerTarget == null) return false;

        Vector2 monsterPos = transform.position;
        Vector2 playerPos = playerTarget.position;

        // 플레이어와 몬스터 사이의 거리(직선 거리) 계산
        float distance = Vector2.Distance(playerPos, monsterPos);

        // 계산된 거리가 attackRange 이내인지 확인
        return distance <= attackRange;
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }

    // --- Health System Methods ---

    public void TakeDamage(float damage)
    {
        if (IsDead()) return; // 이미 죽은 상태면 대미지 처리하지 않음

        CurrentHP -= damage; // HP 감소

        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {CurrentHP}");

        if (CurrentHP <= 0)
        {
            // HP가 0 이하면 MonsterStateManager에서 DEAT 상태로 자동 전환
            Debug.Log($"{gameObject.name} has been defeated!");
        }
        else
        {
            // 피격 애니메이션 또는 이펙트 재생
            // 피격 상태로 전환 (짧게 Transition)
            monsterStateManager.ChangeState(MonsterState.HIT);
        }
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }
}
