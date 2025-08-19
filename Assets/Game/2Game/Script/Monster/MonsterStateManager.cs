using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    private Dictionary<MonsterState, IState<Monster>> monsterStatePool;
    private IState<Monster> currentState;
    private IState<Monster> previousState; // 이전 상태를 저장할 변수
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

    // GetComponent<T>() 또는 AddComponent<T>()를 유연하게 처리하는 헬퍼 메서드
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
        Debug.Log("현재상태 : " + pType.ToString());
        if (CurrentMonsterState == pType && CurrentState != null) // 같은 상태로의 전이 방지 (선택 사항)
        {
            Debug.LogWarning($"같은 상태론 변경 할 수 없습니다. : {pType}");
            return;
        }

        CurrentState?.Exit();
        previousState = CurrentState; // 현재 상태를 이전 상태로 저장

        CurrentState = monsterStatePool[pType];
        CurrentMonsterState = pType;

        CurrentState?.Enter();
        CurrentState?.Action();
    }

    //이전 상태로 되돌리기
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

    // --- 상태 전이 로직을 추가할 곳 ---
    private void Update()
    {
        // 몬스터가 null이거나 죽은 상태면 로직을 수행하지 않습니다.
        if (monster == null || monster.IsDead())
        {
            // Debug.Log("Monster is dead or null, skipping state transitions.");
            return;
        }

        // 1. 타겟 탐지 (IDLE 상태에서만 탐색하도록 제한할 수 있습니다. 아니면 항상 탐색.)
        // 여기서는 모든 상태에서 플레이어를 지속적으로 탐색하도록 합니다.
        // monster.FindNearestPlayerTarget(); // Monster 클래스에 이 메서드가 있다고 가정

        // 2. 공통 상태 전이 로직 (가장 높은 우선순위부터 체크)

        // 사망 상태로의 전이는 항상 최우선으로 처리
        if (monster.CurrentHP <= 0 && CurrentMonsterState != MonsterState.DEATH)
        {
            ChangeState(MonsterState.DEATH);
            return;
        }

        // 피격 상태로의 전이 (외부에서 Hit() 호출 시 처리되므로 여기서는 생략 가능하지만,
        // 강제로 특정 조건을 만족하면 Hit 상태로 가는 로직을 넣을 수도 있습니다.)
        // 예: 특정 디버프를 받으면 HIT 상태로...
        // if (monster.IsStunned() && CurrentMonsterState != MonsterState.HIT)
        // {
        //     ChangeState(MonsterState.HIT);
        //     return;
        // }


        // 타겟이 있는지 여부와 거리 기반 상태 전이
        if (monster.HasTarget())
        {
            if (monster.IsTargetInAttackRange())
            {
                if (CurrentMonsterState != MonsterState.ATTACK) // 현재 상태가 공격이 아니라면
                {
                    ChangeState(MonsterState.ATTACK);
                }
            }
            else if (monster.IsTargetInChaseRange())
            {
                if (CurrentMonsterState != MonsterState.MOVE) // 현재 상태가 이동이 아니라면
                {
                    ChangeState(MonsterState.MOVE);
                }
            }
            else // 타겟은 있지만 추적/공격 범위 밖이라면
            {
                if (CurrentMonsterState != MonsterState.IDLE) // 현재 상태가 Idle이 아니라면
                {
                    ChangeState(MonsterState.IDLE); // 다시 Idle 상태로 돌아가도록
                }
            }
        }
        else // 타겟을 잃었을 경우 (HasTarget()이 false일 때)
        {
            if (CurrentMonsterState != MonsterState.IDLE) // 현재 상태가 Idle이 아니라면
            {
                ChangeState(MonsterState.IDLE); // Idle 상태로 전환
            }
        }

        // 현재 상태의 Action을 매 프레임 호출 (해당 상태의 주기적 로직 실행)
        // MonsterState별로 Action이 매 프레임 실행될 필요가 없다면 (코루틴 사용 시) 제거해도 됩니다.
        // 예를 들어 MonsterMoveState의 Action은 이동 로직 코루틴을 시작하고,
        // 그 코루틴이 끝날 때까지 기다리도록 설계할 수 있습니다.
        CurrentState?.Action();
    }
}
