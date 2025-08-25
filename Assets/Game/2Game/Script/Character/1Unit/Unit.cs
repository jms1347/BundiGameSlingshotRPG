using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("공통 매니져 변수")]
    [SerializeField] protected UnitStats stats; // UnitStats 클래스 사용 가정
    [SerializeField] protected UnitStateManager unitStateManager;

    [Header("애니메이션")]
    [SerializeField] protected Animator animator;

    [Header("타켓")]
    [SerializeField] protected Transform target;
    [SerializeField] private string searchTag;

    [Header("외부 접근 변수")]
    public Animator Animator { get => animator; set => animator = value; }
    public UnitStateManager UnitStateManager { get => unitStateManager; set => unitStateManager = value; }
    public Transform Target { get => target; set => target = value; }
    public UnitStats Stats { get => stats; set => stats = value; }
    public string SearchTag { get => searchTag; set => searchTag = value; }

    private void Awake()
    {
        // 1. UnitStats 초기화
        if (stats == null)
        {
            // 예시: MaxHP, 추적범위, 공격범위, 공격력, 공격속도, 방어력, 이동속도
            stats = new UnitStats(100f, 10f, 2f, 5f, 3f, 0f, 10f);
        }
        //stats.InitStats(); // UnitStats 내부에서 CurrentHP = MaxHP; 등의 초기화 수행

        // 2. UnitStateManager 초기화
        unitStateManager = GetComponent<UnitStateManager>();
        if (unitStateManager == null)
        {
            unitStateManager = gameObject.AddComponent<UnitStateManager>();
        }
        // UnitStateManager에게 이 Unit 인스턴스를 'Context'로 설정하도록 전달합니다.
        unitStateManager.SettingContext(this);

        // 3. Animator 컴포넌트 초기화 (선택 사항)
        if (animator == null)
        {
            animator = this.transform.GetChild(0).GetComponent<Animator>();
        }
    }
    private void Start()
    {
        unitStateManager.ChangeState(UnitState.IDLE);
    }

    public virtual void TakeDamage(float rawDamage)
    {
        if (IsDead()) return; // 이미 죽었으면 더 이상 피해를 입지 않습니다.

        // 방어력 적용 로직 (예시: 방어력에 비례하여 피해 감소)
        float finalDamage = Mathf.Max(0, rawDamage - stats.DefenseArmor);

        stats.TakeDamage(finalDamage); // UnitStats에서 체력 감소 처리 (이때 OnHealthChanged 이벤트 발생)

        Debug.Log($"{gameObject.name} took {finalDamage} final damage (raw: {rawDamage}). Current HP: {stats.CurrentHP}");

        if (IsDead())
        {
            Die(); // 체력이 0 이하면 사망 처리
        }
        else
        {
            // 사망하지 않았다면, 피격 상태로 전환 요청 (StateManager에게 지시)
            unitStateManager?.ChangeState(UnitState.DAMAGEHIT);
        }
    }

    public virtual void Die()
    {
        // 이미 죽은 상태면 중복 호출 방지
        if (IsDead()) return;

        Debug.Log($"{gameObject.name} has died.");
        // 사망 상태로 전환 요청 (StateManager에게 지시)
        unitStateManager?.ChangeState(UnitState.DEATH);

        // 여기서 유닛 제거, 아이템 드롭, 사망 애니메이션 종료 등 추가 사망 로직을 수행할 수 있습니다.
        // 예: GameObject.Destroy(gameObject, 3f); // 3초 후 오브젝트 파괴
    }

    public bool IsDead()
    {
        return stats.IsDead(); // UnitStats의 IsDead 메서드를 사용합니다.
    }

    public virtual bool HasTarget()
    {
        return target != null;
    }

    public virtual bool IsTargetInAttackRange()
    {
        if (!HasTarget()) return false;
        // 2D 게임이라면 Vector2.Distance, 3D라면 Vector3.Distance를 사용합니다.
        return Vector3.Distance(transform.position, target.position) <= Stats.AttackRange;
    }

    public virtual bool IsTargetInChaseRange()
    {
        if (!HasTarget()) return false;
        return Vector3.Distance(transform.position, target.position) <= Stats.ChaseRange;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        // Debug.Log($"{gameObject.name} set target to {target?.name ?? "null"}");
    }

    public virtual void FindNearestTarget(string targetTag)
    {
        // 씬에서 모든 해당 태그를 가진 오브젝트를 찾습니다. (성능상 무거운 작업이므로 너무 자주 호출하지 않도록 주의)
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity; // 가장 짧은 거리를 찾기 위한 초기값

        foreach (GameObject obj in targets)
        {
            // 대상이 유효한지, 그리고 스스로가 아닌지 확인합니다.
            if (obj == null || obj == this.gameObject) continue;

            // 대상이 Unit 컴포넌트를 가지고 있고, 아직 살아있는지 확인합니다.
            Unit potentialTargetUnit = obj.GetComponent<Unit>();
            if (potentialTargetUnit != null && potentialTargetUnit.IsDead())
            {
                continue; // 죽은 대상은 건너뜁니다.
            }

            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // 현재까지 찾은 가장 가까운 대상보다 더 가까우면 업데이트합니다.
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = obj.transform;
            }
        }

        // 가장 가까운 대상을 찾았으면 Unit의 target으로 설정합니다.
        SetTarget(nearestTarget);

        // 디버깅용 로그 (선택 사항)
        // if (nearestTarget != null)
        // {
        //     Debug.Log($"{gameObject.name} found nearest target: {nearestTarget.name} at distance {minDistance:F2}");
        // }
        // else
        // {
        //     Debug.Log($"{gameObject.name} could not find any target with tag: {targetTag}");
        // }
    }

}
