using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("���� �Ŵ��� ����")]
    [SerializeField] protected UnitStats stats; // UnitStats Ŭ���� ��� ����
    [SerializeField] protected UnitStateManager unitStateManager;

    [Header("�ִϸ��̼�")]
    [SerializeField] protected Animator animator;

    [Header("Ÿ��")]
    [SerializeField] protected Transform target;
    [SerializeField] private string searchTag;

    [Header("�ܺ� ���� ����")]
    public Animator Animator { get => animator; set => animator = value; }
    public UnitStateManager UnitStateManager { get => unitStateManager; set => unitStateManager = value; }
    public Transform Target { get => target; set => target = value; }
    public UnitStats Stats { get => stats; set => stats = value; }
    public string SearchTag { get => searchTag; set => searchTag = value; }

    private void Awake()
    {
        // 1. UnitStats �ʱ�ȭ
        if (stats == null)
        {
            // ����: MaxHP, ��������, ���ݹ���, ���ݷ�, ���ݼӵ�, ����, �̵��ӵ�
            stats = new UnitStats(100f, 10f, 2f, 5f, 3f, 0f, 10f);
        }
        //stats.InitStats(); // UnitStats ���ο��� CurrentHP = MaxHP; ���� �ʱ�ȭ ����

        // 2. UnitStateManager �ʱ�ȭ
        unitStateManager = GetComponent<UnitStateManager>();
        if (unitStateManager == null)
        {
            unitStateManager = gameObject.AddComponent<UnitStateManager>();
        }
        // UnitStateManager���� �� Unit �ν��Ͻ��� 'Context'�� �����ϵ��� �����մϴ�.
        unitStateManager.SettingContext(this);

        // 3. Animator ������Ʈ �ʱ�ȭ (���� ����)
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
        if (IsDead()) return; // �̹� �׾����� �� �̻� ���ظ� ���� �ʽ��ϴ�.

        // ���� ���� ���� (����: ���¿� ����Ͽ� ���� ����)
        float finalDamage = Mathf.Max(0, rawDamage - stats.DefenseArmor);

        stats.TakeDamage(finalDamage); // UnitStats���� ü�� ���� ó�� (�̶� OnHealthChanged �̺�Ʈ �߻�)

        Debug.Log($"{gameObject.name} took {finalDamage} final damage (raw: {rawDamage}). Current HP: {stats.CurrentHP}");

        if (IsDead())
        {
            Die(); // ü���� 0 ���ϸ� ��� ó��
        }
        else
        {
            // ������� �ʾҴٸ�, �ǰ� ���·� ��ȯ ��û (StateManager���� ����)
            unitStateManager?.ChangeState(UnitState.DAMAGEHIT);
        }
    }

    public virtual void Die()
    {
        // �̹� ���� ���¸� �ߺ� ȣ�� ����
        if (IsDead()) return;

        Debug.Log($"{gameObject.name} has died.");
        // ��� ���·� ��ȯ ��û (StateManager���� ����)
        unitStateManager?.ChangeState(UnitState.DEATH);

        // ���⼭ ���� ����, ������ ���, ��� �ִϸ��̼� ���� �� �߰� ��� ������ ������ �� �ֽ��ϴ�.
        // ��: GameObject.Destroy(gameObject, 3f); // 3�� �� ������Ʈ �ı�
    }

    public bool IsDead()
    {
        return stats.IsDead(); // UnitStats�� IsDead �޼��带 ����մϴ�.
    }

    public virtual bool HasTarget()
    {
        return target != null;
    }

    public virtual bool IsTargetInAttackRange()
    {
        if (!HasTarget()) return false;
        // 2D �����̶�� Vector2.Distance, 3D��� Vector3.Distance�� ����մϴ�.
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
        // ������ ��� �ش� �±׸� ���� ������Ʈ�� ã���ϴ�. (���ɻ� ���ſ� �۾��̹Ƿ� �ʹ� ���� ȣ������ �ʵ��� ����)
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity; // ���� ª�� �Ÿ��� ã�� ���� �ʱⰪ

        foreach (GameObject obj in targets)
        {
            // ����� ��ȿ����, �׸��� �����ΰ� �ƴ��� Ȯ���մϴ�.
            if (obj == null || obj == this.gameObject) continue;

            // ����� Unit ������Ʈ�� ������ �ְ�, ���� ����ִ��� Ȯ���մϴ�.
            Unit potentialTargetUnit = obj.GetComponent<Unit>();
            if (potentialTargetUnit != null && potentialTargetUnit.IsDead())
            {
                continue; // ���� ����� �ǳʶݴϴ�.
            }

            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // ������� ã�� ���� ����� ��󺸴� �� ������ ������Ʈ�մϴ�.
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = obj.transform;
            }
        }

        // ���� ����� ����� ã������ Unit�� target���� �����մϴ�.
        SetTarget(nearestTarget);

        // ������ �α� (���� ����)
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
