using DG.Tweening;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState
    {
        Idle,      // 대기
        Stun,      // 스턴
        Move,      // 이동
        Attack,    // 공격
        Dead       // 사망
    }

    public enum UnitType
    {
        Melee = 0,
        Ranged = 1,
        Horse = 2
    }

    public UnitType unityType;

    [Header("바 관련")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    private Color originalColor;

    [Header("써치 관련")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("공격/방어 관련")]
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float currentAttackCooltime;
    [SerializeField] private float defenseArmor;

    [Header("이동속도 관련")]
    [SerializeField] private float moveSpeed;

    [Header("상태")]
    [SerializeField] private UnitState currentState;
    private bool isAttacking = false;   //공격 플래그
    private bool isDying = false;   //사망 플래그

    [Header("미사일 관련")]
    [SerializeField] private Missile[] missiles;

    [Header("타켓")]
    [SerializeField] private string searchTag;
    [SerializeField] protected Transform target;

    [Header("컴포넌트 변수")]
    private SpriteRenderer spriteRenderer;

    [Header("외부 접근 변수")]
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, maxHp);
    }

    public float MaxMp { get => maxMp; set => maxMp = value; }
    public float CurrentMp
    {
        get => currentMp;
        set => currentMp = Mathf.Clamp(value, 0, maxMp);
    }

    public float AttackPower { get => attackPower; set => attackPower = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ChaseRange { get => chaseRange; set => chaseRange = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float DefenseArmor { get => defenseArmor; set => defenseArmor = value; }
    public Transform Target { get => target; set => target = value; }
    public string SearchTag { get => searchTag; set => searchTag = value; }
    public UnitState CurrentState { get => currentState; set
        {
            currentState = value;
        }
    }
    private void Awake()
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color; // 유닛의 원래 색상 저장
            }

        }
    }
    public void Start()
    {
        CurrentState = UnitState.Idle;
    }

    private void Update()
    {
        SetState();
    }
    public void SetState()
    {
        if (isDying) return;
        if (currentState == UnitState.Stun) return;

        // 1. 현재 타겟의 유효성 검사 (매 프레임)
        if (target != null)
        {
            // 타겟이 죽었거나 추격 범위를 벗어났으면 타겟을 잃음
            if (target.GetComponent<UnitBase>().IsDead() || Vector3.Distance(transform.position, target.position) > chaseRange)
            {
                SetTarget(null);
            }
        }

        // 2. 새로운 최적의 타겟 탐색 (타겟이 없거나, 말(Horse) 유닛일 경우)
        // 이 로직을 상단에 두어 항상 최적의 타겟을 찾도록 합니다.
        Transform newBestTarget = null;
        if (unityType == UnitType.Horse)
        {
            // 말(Horse) 유닛은 원거리 우선 탐색
            newBestTarget = FindNearestHorseTargetWithReturnValue();
        }
        else
        {
            // 그 외 유닛은 가장 가까운 적 탐색
            newBestTarget = FindNearestTargetWithReturnValue(searchTag);
        }

        // 3. 현재 타겟과 새로운 타겟 비교
        if (newBestTarget != null)
        {
            // 현재 타겟이 없거나, 새로운 타겟이 더 가까울 경우 타겟 변경
            if (target == null || Vector3.Distance(transform.position, newBestTarget.position) < Vector3.Distance(transform.position, target.position))
            {
                SetTarget(newBestTarget);
            }
        }
        else
        {
            // 주변에 타겟이 없으면 타겟을 null로 설정
            SetTarget(null);
        }

        // 4. 상태 전환 로직
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                currentState = UnitState.Attack;
            }
            else
            {
                currentState = UnitState.Move;
            }
        }
        else
        {
            currentState = UnitState.Idle;
        }

        // 상태별 행동 로직은 기존과 동일
        switch (currentState)
        {
            case UnitState.Idle:
                // 위에서 이미 타겟 탐색 로직을 처리했으므로 별도 로직은 필요 없음
                break;

            case UnitState.Move:
                // 타겟이 유효한지 체크
                if (target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (distanceToTarget <= attackRange)
                    {
                        currentState = UnitState.Attack;
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(
                            transform.position,
                            target.position,
                            moveSpeed * Time.deltaTime
                        );
                    }
                }
                break;
            case UnitState.Attack:
                if (target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (distanceToTarget <= attackRange)
                    {
                        if (!isAttacking)
                        {
                            currentAttackCooltime -= Time.deltaTime;
                        }
                        if (currentAttackCooltime <= 0 && !isAttacking)
                        {
                            isAttacking = true;
                            if (unityType == UnitType.Melee || unityType == UnitType.Horse)
                            {
                                MeleeAttack(target);
                            }
                            else if (unityType == UnitType.Ranged)
                            {
                                RangedAttack(target);
                            }
                            currentAttackCooltime = attackSpeed;
                        }
                    }
                    else
                    {
                        // 타겟이 공격 범위를 벗어나면 Move 상태로 전환
                        currentState = UnitState.Move;
                    }
                }
                break;
            case UnitState.Stun:
                break;
            case UnitState.Dead:
                break;
        }
    }

    #region 공격 모션
    //근거리 공격
    private void MeleeAttack(Transform targetTransform)
    {
        // 공격 중이므로 추가 공격 방지
        isAttacking = true;

        // 현재 위치를 저장 (복귀 지점)
        Vector3 originalPosition = transform.position;

        // 1. 타겟 방향으로 반대(뒤)로 이동하는 위치 계산
        Vector3 backwardPosition = Vector3.MoveTowards(originalPosition, targetTransform.position, -0.2f);

        // 2. 타겟과 나의 중간 지점 계산
        Vector3 middlePosition = (originalPosition + targetTransform.position) / 2f;

        var attackSequence = DOTween.Sequence();

        // 단계 1: 반동 모션 (뒤로 살짝 이동)
        attackSequence.Append(transform.DOMove(backwardPosition, 0.1f));

        // 단계 2: 박치기 모션 (중간 지점으로 빠르게 이동)
        attackSequence.Append(transform.DOMove(middlePosition, 0.2f));

        // 단계 3: 원래 위치로 복귀하면서 크기도 원래대로
        attackSequence.Append(transform.DOMove(originalPosition, 0.1f));
        attackSequence.Join(transform.DOScale(1f, 0.1f)); // 이동과 동시에 스케일 변경

        // 애니메이션이 모두 끝난 후 실행될 콜백 함수
        attackSequence.OnComplete(() => {
            // 타겟에게 데미지 적용
            if (targetTransform != null)
            {
                UnitBase targetUnit = targetTransform.GetComponent<UnitBase>();
                if (targetUnit != null)
                {
                    targetUnit.TakeDamage(attackPower);
                }
            }

            // 공격 상태 플래그 비활성화
            isAttacking = false;
        });
    }

    //원거리 공격
    private void RangedAttack(Transform targetTransform)
    {
        for (int i = 0; i < missiles.Length; i++)
        {
            if (!missiles[i].gameObject.activeSelf)
            {
                missiles[i].Launch(targetTransform, attackPower, this.transform.position, this.transform.tag);
                isAttacking = false;

                break;
            }
        }
    }
    #endregion
    #region 타겟 검색 및 타켓 설정
    // 모든 유닛이 사용하는 기본 타겟 탐색 함수 (반환 값 포함)
    public virtual Transform FindNearestTargetWithReturnValue(string targetTag)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, chaseRange);
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == this.gameObject) continue;
            if (collider.CompareTag(targetTag))
            {
                UnitBase targetUnitBase = collider.GetComponent<UnitBase>();
                if (targetUnitBase != null && !targetUnitBase.IsDead())
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTarget = collider.transform;
                    }
                }
            }
        }
        return nearestTarget;
    }

    // 말(Horse) 타입 전용 타겟 탐색 함수 (반환 값 포함)
    public Transform FindNearestHorseTargetWithReturnValue()
    {
        Transform rangedTarget = FindTargetByType(UnitType.Ranged);
        if (rangedTarget != null)
        {
            return rangedTarget;
        }
        else
        {
            return FindNearestTargetWithReturnValue(searchTag);
        }
    }

    // 기존 FindNearestTarget, FindNearestHorseTarget 함수는 제거 또는 리팩토링 필요

    // 특정 UnitType에 해당하는 가장 가까운 대상을 찾는 함수 (기존과 동일)
    private Transform FindTargetByType(UnitType type)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, chaseRange);
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(searchTag))
            {
                UnitBase targetUnitBase = collider.GetComponent<UnitBase>();
                if (targetUnitBase != null && !targetUnitBase.IsDead() && targetUnitBase.unityType == type)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTarget = collider.transform;
                    }
                }
            }
        }
        return nearestTarget;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    #endregion
    #region 피격 효과 (일단 트윈으로)
    private void HitEffect()
    {
        if (spriteRenderer == null) return;

        // 1. 색상을 빨간색으로 즉시 변경
        spriteRenderer.color = Color.red;

        // 2. 0.2초에 걸쳐 원래 색상으로 서서히 돌아오는 연출
        spriteRenderer.DOColor(originalColor, 0.2f);
    }
    #endregion

    #region
    public void Die()
    {
        isDying = true; // 사망 플래그 활성화
        currentState = UnitState.Dead; // 상태를 Dead로 변경

        // 유닛의 모든 행동을 즉시 중단
        StopAllCoroutines(); // 혹시 실행 중인 코루틴이 있다면 모두 중지
                             // DOTween 애니메이션이 있다면 중지
        transform.DOKill();

        // TODO: 사망 연출 (예: 스프라이트가 투명해지거나 넉백 후 사라짐)
        // DOTween을 사용하여 투명하게 사라지는 연출
        spriteRenderer.DOFade(0, 1.0f).OnComplete(() =>
        {
            // 1초 후 연출이 끝나면 오브젝트 비활성화
            gameObject.SetActive(false);
        });

        // TODO: 사망 시 필요한 다른 로직 추가
        // - UI (예: 체력 바) 숨기기
        // - 적 유닛이라면 플레이어에게 경험치/골드 지급
        // - 드롭 아이템 생성
    }
    #endregion
    #region 체력과 마나 관련
    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        // 디버그용 로그
        LogUtil.Log($"{gameObject.name} took {damage} damage. Current HP: {CurrentHp}");

        // 피격 시 색상 변경 연출
        HitEffect();

        // HP가 0 이하면 사망 처리
        if (IsDead())
        {
            // 사망 상태로 전환하거나 오브젝트 파괴 등
            LogUtil.Log($"{gameObject.name} is dead!");
        }
    }
    // 회복 메서드
    public void Heal(float healAmount)
    {
        CurrentHp += healAmount;
    }

    // 마나 사용 메서드
    public void UseMana(float manaCost)
    {
        CurrentMp -= manaCost;
    }

    // 마나 회복 메서드
    public void ManaRegen(float regenAmount)
    {
        CurrentMp += regenAmount;
    }
    #endregion

    #region 체크 함수
    public bool IsDead()
    {
        return CurrentHp <= 0;
    }
    #endregion
}
