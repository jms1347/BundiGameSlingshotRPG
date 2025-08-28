using DG.Tweening;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState
    {
        Idle,      // ���
        Stun,      // ����
        Move,      // �̵�
        Attack,    // ����
        Dead       // ���
    }

    public enum UnitType
    {
        Melee = 0,
        Ranged = 1,
        Horse = 2
    }

    public UnitType unityType;

    [Header("�� ����")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    private Color originalColor;

    [Header("��ġ ����")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("����/��� ����")]
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float currentAttackCooltime;
    [SerializeField] private float defenseArmor;

    [Header("�̵��ӵ� ����")]
    [SerializeField] private float moveSpeed;

    [Header("����")]
    [SerializeField] private UnitState currentState;
    private bool isAttacking = false;   //���� �÷���
    private bool isDying = false;   //��� �÷���

    [Header("�̻��� ����")]
    [SerializeField] private Missile[] missiles;

    [Header("Ÿ��")]
    [SerializeField] private string searchTag;
    [SerializeField] protected Transform target;

    [Header("������Ʈ ����")]
    private SpriteRenderer spriteRenderer;

    [Header("�ܺ� ���� ����")]
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
                originalColor = spriteRenderer.color; // ������ ���� ���� ����
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

        // 1. ���� Ÿ���� ��ȿ�� �˻� (�� ������)
        if (target != null)
        {
            // Ÿ���� �׾��ų� �߰� ������ ������� Ÿ���� ����
            if (target.GetComponent<UnitBase>().IsDead() || Vector3.Distance(transform.position, target.position) > chaseRange)
            {
                SetTarget(null);
            }
        }

        // 2. ���ο� ������ Ÿ�� Ž�� (Ÿ���� ���ų�, ��(Horse) ������ ���)
        // �� ������ ��ܿ� �ξ� �׻� ������ Ÿ���� ã���� �մϴ�.
        Transform newBestTarget = null;
        if (unityType == UnitType.Horse)
        {
            // ��(Horse) ������ ���Ÿ� �켱 Ž��
            newBestTarget = FindNearestHorseTargetWithReturnValue();
        }
        else
        {
            // �� �� ������ ���� ����� �� Ž��
            newBestTarget = FindNearestTargetWithReturnValue(searchTag);
        }

        // 3. ���� Ÿ�ٰ� ���ο� Ÿ�� ��
        if (newBestTarget != null)
        {
            // ���� Ÿ���� ���ų�, ���ο� Ÿ���� �� ����� ��� Ÿ�� ����
            if (target == null || Vector3.Distance(transform.position, newBestTarget.position) < Vector3.Distance(transform.position, target.position))
            {
                SetTarget(newBestTarget);
            }
        }
        else
        {
            // �ֺ��� Ÿ���� ������ Ÿ���� null�� ����
            SetTarget(null);
        }

        // 4. ���� ��ȯ ����
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

        // ���º� �ൿ ������ ������ ����
        switch (currentState)
        {
            case UnitState.Idle:
                // ������ �̹� Ÿ�� Ž�� ������ ó�������Ƿ� ���� ������ �ʿ� ����
                break;

            case UnitState.Move:
                // Ÿ���� ��ȿ���� üũ
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
                        // Ÿ���� ���� ������ ����� Move ���·� ��ȯ
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

    #region ���� ���
    //�ٰŸ� ����
    private void MeleeAttack(Transform targetTransform)
    {
        // ���� ���̹Ƿ� �߰� ���� ����
        isAttacking = true;

        // ���� ��ġ�� ���� (���� ����)
        Vector3 originalPosition = transform.position;

        // 1. Ÿ�� �������� �ݴ�(��)�� �̵��ϴ� ��ġ ���
        Vector3 backwardPosition = Vector3.MoveTowards(originalPosition, targetTransform.position, -0.2f);

        // 2. Ÿ�ٰ� ���� �߰� ���� ���
        Vector3 middlePosition = (originalPosition + targetTransform.position) / 2f;

        var attackSequence = DOTween.Sequence();

        // �ܰ� 1: �ݵ� ��� (�ڷ� ��¦ �̵�)
        attackSequence.Append(transform.DOMove(backwardPosition, 0.1f));

        // �ܰ� 2: ��ġ�� ��� (�߰� �������� ������ �̵�)
        attackSequence.Append(transform.DOMove(middlePosition, 0.2f));

        // �ܰ� 3: ���� ��ġ�� �����ϸ鼭 ũ�⵵ �������
        attackSequence.Append(transform.DOMove(originalPosition, 0.1f));
        attackSequence.Join(transform.DOScale(1f, 0.1f)); // �̵��� ���ÿ� ������ ����

        // �ִϸ��̼��� ��� ���� �� ����� �ݹ� �Լ�
        attackSequence.OnComplete(() => {
            // Ÿ�ٿ��� ������ ����
            if (targetTransform != null)
            {
                UnitBase targetUnit = targetTransform.GetComponent<UnitBase>();
                if (targetUnit != null)
                {
                    targetUnit.TakeDamage(attackPower);
                }
            }

            // ���� ���� �÷��� ��Ȱ��ȭ
            isAttacking = false;
        });
    }

    //���Ÿ� ����
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
    #region Ÿ�� �˻� �� Ÿ�� ����
    // ��� ������ ����ϴ� �⺻ Ÿ�� Ž�� �Լ� (��ȯ �� ����)
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

    // ��(Horse) Ÿ�� ���� Ÿ�� Ž�� �Լ� (��ȯ �� ����)
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

    // ���� FindNearestTarget, FindNearestHorseTarget �Լ��� ���� �Ǵ� �����丵 �ʿ�

    // Ư�� UnitType�� �ش��ϴ� ���� ����� ����� ã�� �Լ� (������ ����)
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
    #region �ǰ� ȿ�� (�ϴ� Ʈ������)
    private void HitEffect()
    {
        if (spriteRenderer == null) return;

        // 1. ������ ���������� ��� ����
        spriteRenderer.color = Color.red;

        // 2. 0.2�ʿ� ���� ���� �������� ������ ���ƿ��� ����
        spriteRenderer.DOColor(originalColor, 0.2f);
    }
    #endregion

    #region
    public void Die()
    {
        isDying = true; // ��� �÷��� Ȱ��ȭ
        currentState = UnitState.Dead; // ���¸� Dead�� ����

        // ������ ��� �ൿ�� ��� �ߴ�
        StopAllCoroutines(); // Ȥ�� ���� ���� �ڷ�ƾ�� �ִٸ� ��� ����
                             // DOTween �ִϸ��̼��� �ִٸ� ����
        transform.DOKill();

        // TODO: ��� ���� (��: ��������Ʈ�� ���������ų� �˹� �� �����)
        // DOTween�� ����Ͽ� �����ϰ� ������� ����
        spriteRenderer.DOFade(0, 1.0f).OnComplete(() =>
        {
            // 1�� �� ������ ������ ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
        });

        // TODO: ��� �� �ʿ��� �ٸ� ���� �߰�
        // - UI (��: ü�� ��) �����
        // - �� �����̶�� �÷��̾�� ����ġ/��� ����
        // - ��� ������ ����
    }
    #endregion
    #region ü�°� ���� ����
    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        // ����׿� �α�
        LogUtil.Log($"{gameObject.name} took {damage} damage. Current HP: {CurrentHp}");

        // �ǰ� �� ���� ���� ����
        HitEffect();

        // HP�� 0 ���ϸ� ��� ó��
        if (IsDead())
        {
            // ��� ���·� ��ȯ�ϰų� ������Ʈ �ı� ��
            LogUtil.Log($"{gameObject.name} is dead!");
        }
    }
    // ȸ�� �޼���
    public void Heal(float healAmount)
    {
        CurrentHp += healAmount;
    }

    // ���� ��� �޼���
    public void UseMana(float manaCost)
    {
        CurrentMp -= manaCost;
    }

    // ���� ȸ�� �޼���
    public void ManaRegen(float regenAmount)
    {
        CurrentMp += regenAmount;
    }
    #endregion

    #region üũ �Լ�
    public bool IsDead()
    {
        return CurrentHp <= 0;
    }
    #endregion
}
