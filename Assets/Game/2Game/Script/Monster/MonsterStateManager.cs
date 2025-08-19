using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    private Dictionary<MonsterState, IState<Monster>> monsterStatePool;
    private IState<Monster> currentState;
    private IState<Monster> previousState; // ���� ���¸� ������ ����
    private MonsterState currentMonsterState;
    private Monster monster;

    public IState<Monster> CurrentState { get => currentState; set => currentState = value; }
    public MonsterState CurrentMonsterState { get => currentMonsterState; set => currentMonsterState = value; }
    public Dictionary<MonsterState, IState<Monster>> MonsterStatePool { get => monsterStatePool; set => monsterStatePool = value; }

    public void SetMonster(Monster pMonster)
    {
        monster = pMonster;
    }

    private void Start()
    {
        InitStatePool();

        ChangeState(MonsterState.IDLE);
    }

    public void InitStatePool()
    {
        monsterStatePool = new Dictionary<MonsterState, IState<Monster>>();
        monsterStatePool[MonsterState.IDLE] = gameObject.AddComponent<MonsterIdleState>();
        monsterStatePool[MonsterState.MOVE] = gameObject.AddComponent<MonsterMoveState>();
        monsterStatePool[MonsterState.ATTACK] = gameObject.AddComponent<MonsterAttackState>();
        monsterStatePool[MonsterState.HIT] = gameObject.AddComponent<MonsterDamageHitState>();
        monsterStatePool[MonsterState.DEATH] = gameObject.AddComponent<MonsterDeathState>();

        foreach (var i in monsterStatePool)
        {           
            monsterStatePool[i.Key]?.Handle(monster);
        }
    }

    // GetComponent<T>() �Ǵ� AddComponent<T>()�� �����ϰ� ó���ϴ� ���� �޼���
    private T GetOrAddComponent<T>() where T : Component, IState<Monster>
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    public void ChangeState(MonsterState pType)
    {
        Debug.Log("������� : " + pType.ToString());
        if (CurrentMonsterState == pType && CurrentState != null) // ���� ���·��� ���� ���� (���� ����)
        {
            Debug.LogWarning($"���� ���·� ���� �� �� �����ϴ�. : {pType}");
            return;
        }

        CurrentState?.Exit();
        previousState = CurrentState; // ���� ���¸� ���� ���·� ����

        CurrentState = monsterStatePool[pType];
        CurrentMonsterState = pType;

        CurrentState?.Enter();
        CurrentState?.Action();
    }

    //���� ���·� �ǵ�����
    public void RevertToPreviousState()
    {
        if (previousState != null && previousState != CurrentState)
        {
            ChangeState(GetMonsterStateFromIState(previousState)); // You'll need a way to map IState back to MonsterState
        }
    }

    private MonsterState GetMonsterStateFromIState(IState<Monster> state)
    {
        foreach (var pair in monsterStatePool)
        {
            if (pair.Value == state)
            {
                return pair.Key;
            }
        }
        Debug.LogError("Could not find MonsterState for given IState!");
        return MonsterState.IDLE; // Fallback
    }

    public bool IsCurrentState(MonsterState pState)
    {
        if (currentState == monsterStatePool[pState])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // --- ���� ���� ������ �߰��� �� ---
    private void Update()
    {
        // ���Ͱ� null�̰ų� ���� ���¸� ������ �������� �ʽ��ϴ�.
        if (monster == null || monster.IsDead())
        {
            // Debug.Log("Monster is dead or null, skipping state transitions.");
            return;
        }

        // 1. Ÿ�� Ž�� (IDLE ���¿����� Ž���ϵ��� ������ �� �ֽ��ϴ�. �ƴϸ� �׻� Ž��.)
        // ���⼭�� ��� ���¿��� �÷��̾ ���������� Ž���ϵ��� �մϴ�.
        // monster.FindNearestPlayerTarget(); // Monster Ŭ������ �� �޼��尡 �ִٰ� ����

        // 2. ���� ���� ���� ���� (���� ���� �켱�������� üũ)

        // ��� ���·��� ���̴� �׻� �ֿ켱���� ó��
        if (monster.CurrentHP <= 0 && CurrentMonsterState != MonsterState.DEATH)
        {
            ChangeState(MonsterState.DEATH);
            return;
        }

        // �ǰ� ���·��� ���� (�ܺο��� Hit() ȣ�� �� ó���ǹǷ� ���⼭�� ���� ����������,
        // ������ Ư�� ������ �����ϸ� Hit ���·� ���� ������ ���� ���� �ֽ��ϴ�.)
        // ��: Ư�� ������� ������ HIT ���·�...
        // if (monster.IsStunned() && CurrentMonsterState != MonsterState.HIT)
        // {
        //     ChangeState(MonsterState.HIT);
        //     return;
        // }


        // Ÿ���� �ִ��� ���ο� �Ÿ� ��� ���� ����
        if (monster.HasTarget())
        {
            if (monster.IsTargetInAttackRange())
            {
                if (CurrentMonsterState != MonsterState.ATTACK) // ���� ���°� ������ �ƴ϶��
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            else if (monster.IsTargetInChaseRange())
            {
                if (CurrentMonsterState != MonsterState.MOVE) // ���� ���°� �̵��� �ƴ϶��
                {
                    ChangeState(MonsterState.MOVE);
                }
            }
            else // Ÿ���� ������ ����/���� ���� ���̶��
            {
                if (CurrentMonsterState != MonsterState.IDLE) // ���� ���°� Idle�� �ƴ϶��
                {
                    ChangeState(MonsterState.IDLE); // �ٽ� Idle ���·� ���ư�����
                }
            }
        }
        else // Ÿ���� �Ҿ��� ��� (HasTarget()�� false�� ��)
        {
            if (CurrentMonsterState != MonsterState.IDLE) // ���� ���°� Idle�� �ƴ϶��
            {
                ChangeState(MonsterState.IDLE); // Idle ���·� ��ȯ
            }
        }

        // ���� ������ Action�� �� ������ ȣ�� (�ش� ������ �ֱ��� ���� ����)
        // MonsterState���� Action�� �� ������ ����� �ʿ䰡 ���ٸ� (�ڷ�ƾ ��� ��) �����ص� �˴ϴ�.
        // ���� ��� MonsterMoveState�� Action�� �̵� ���� �ڷ�ƾ�� �����ϰ�,
        // �� �ڷ�ƾ�� ���� ������ ��ٸ����� ������ �� �ֽ��ϴ�.
        CurrentState?.Action();
    }
}
