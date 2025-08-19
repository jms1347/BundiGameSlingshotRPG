using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster Attributes")]
    public float moveSpeed = 3f;
    public float chaseRange = 15f;
    public float attackRange = 1f;
    //public float rotationSpeed = 10f;

    public float attackDamage = 10f; // ���Ͱ� �ִ� ����� �߰�
    public float attackDuration = 0.2f; // ���ݾִϸ��̼� �ð�
    public float attackCooldown = 3f; // ���ݼӵ�

    [Header("Health")] // HP �ý��� �߰�
    [SerializeField] private float currentHP;
    [SerializeField] private float maxHP = 100f; // �⺻ �ִ� ü��


    [Header("References")]
    public Transform playerTarget; // Assign your Player GameObject here in the Inspector
    [SerializeField] private Animator monsterAni;      // Assign the Monster's Animator component here

    private Vector3 initialPosition; // For potential idle wandering or returning home

    [Header("���� ����")]
    public MonsterStateManager monsterStateManager;

    public Animator MonsterAni { get => monsterAni; set => monsterAni = value; }
    // HP ���� ������Ƽ
    public float CurrentHP
    {
        get => currentHP;
        private set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP); // HP�� 0 �̸�, maxHP �ʰ����� �ʵ��� ����
            if (currentHP <= 0 && !IsDead()) // ���� ���� ����
            {
                monsterStateManager.ChangeState(MonsterState.DEATH);
            }
        }
    }
    public float MaxHP => maxHP;


    void Awake()
    {
        // ���� ���� �Ŵ����� ��ȿ���� Ȯ���ϰ� ����
        if (monsterStateManager == null)
        {
            monsterStateManager = GetComponent<MonsterStateManager>();
            if (monsterStateManager == null)
            {
                Debug.LogError("MonsterStateManager not found on the same GameObject as Monster!", this);
                enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
                return;
            }
        }
        monsterStateManager.SetMonster(this);

        initialPosition = transform.position; // Store initial position
        if (MonsterAni == null)
        {
            MonsterAni = GetComponent<Animator>();
        }

        CurrentHP = maxHP; // ���� ���� �� HP�� �ִ�� ����
        // ���� ���� �� Ÿ���� �Ҵ���� �ʾҴٸ� Player �±׸� ���� ������Ʈ�� ã�� �Ҵ�
        if (playerTarget == null)
        {
            FindPlayerTarget();
        }
    }

    public void FindPlayerTarget()
    {
        // GameObject.FindWithTag�� ����� ���� �۾��̹Ƿ� ���� ȣ������ �ʴ� ���� �����ϴ�.
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
        // 1. Ÿ�� ���� ���
        Vector3 direction = (targetPosition - transform.position).normalized;

        //// 2. Y�� �������θ� ȸ���ϵ��� ���� (���Ͱ� ���ų� ������ �ʰ�)
        //if (direction != Vector3.zero) // 0 ������ �� �Ĵٺ� ������ �����Ƿ� ȸ������ �ʵ��� ����
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    // Slerp�� ����Ͽ� �ε巴�� ȸ��
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
        // 1. Ÿ�� ���� ���
        Vector3 direction = (playerTarget.transform.position - transform.position).normalized;

        //// 2. Y�� �������θ� ȸ���ϵ��� ���� (���Ͱ� ���ų� ������ �ʰ�)
        //if (direction != Vector3.zero) // 0 ������ �� �Ĵٺ� ������ �����Ƿ� ȸ������ �ʵ��� ����
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    // Slerp�� ����Ͽ� �ε巴�� ȸ��
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

    // --- ������ IsTargetInAttackRange �޼��� ---
    public bool IsTargetInAttackRange()
    {
        if (playerTarget == null) return false;

        Vector2 monsterPos = transform.position;
        Vector2 playerPos = playerTarget.position;

        // �÷��̾�� ���� ������ �Ÿ�(���� �Ÿ�) ���
        float distance = Vector2.Distance(playerPos, monsterPos);

        // ���� �Ÿ��� attackRange �̳����� Ȯ��
        return distance <= attackRange;
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }

    // --- Health System Methods ---

    public void TakeDamage(float damage)
    {
        if (IsDead()) return; // �̹� ���� ���¸� ����� ó������ ����

        CurrentHP -= damage; // HP ����

        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {CurrentHP}");

        if (CurrentHP <= 0)
        {
            // HP�� 0 ���ϸ� MonsterStateManager���� DEAT ���·� �ڵ� ��ȯ
            Debug.Log($"{gameObject.name} has been defeated!");
        }
        else
        {
            // �ǰ� �ִϸ��̼� �Ǵ� ����Ʈ ���
            // �ǰ� ���·� ��ȯ (ª�� Transition)
            monsterStateManager.ChangeState(MonsterState.HIT);
        }
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }
}
