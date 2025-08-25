// UnitStateManager.cs
using UnityEngine;
using System.Collections.Generic;

// IState 인터페이스는 Unit 타입으로 정의되어야 합니다.
// public interface IState<T> where T : MonoBehaviour { ... }

// UnitState Enum은 Unit.cs 또는 별도의 GlobalEnums.cs에 정의되어야 합니다.
// public enum UnitState { IDLE, MOVE, ATTACK, HIT, DEATH, ... }

public class UnitStateManager : StateContext<Unit, UnitState>
{
    protected override void InitStatePool()
    {
        base.InitStatePool(); // 부모 클래스의 Dictionary 초기화 호출

        // 여기에 Unit의 구체적인 상태들을 statePool에 추가합니다.
        // 각 상태 클래스는 IState<Unit>을 구현해야 합니다.
        // GetOrAddComponent 헬퍼 메서드를 추가하여 유연하게 처리할 수 있습니다.
        StatePool[UnitState.IDLE] = GetOrAddComponent<UnitIdleState>();
        StatePool[UnitState.MOVE] = GetOrAddComponent<UnitMoveState>();
        StatePool[UnitState.ATTACK] = GetOrAddComponent<UnitAttackState>();
        StatePool[UnitState.DAMAGEHIT] = GetOrAddComponent<UnitDamageHitState>();
        StatePool[UnitState.DEATH] = GetOrAddComponent<UnitDeathState>();

        // 각 상태에 context(Unit)를 전달하여 초기화합니다.
        foreach (var i in StatePool)
        {            
            i.Value?.Handle(Context); 
        }

        ChangeState(UnitState.IDLE);

    }

    // GetComponent<T>() 또는 AddComponent<T>()를 유연하게 처리하는 헬퍼 메서드
    private TState GetOrAddComponent<TState>() where TState : Component, IState<Unit> // IState<Unit> 타입으로 제한
    {
        TState component = GetComponent<TState>();
        if (component == null)
        {
            component = gameObject.AddComponent<TState>();
        }
        return component;
    }

    private void Update()
    {
        if (Context == null || Context.IsDead())
        {
            return;
        }

        if (CurrentState == null) return;

        // 1. 공통 상태 전이 로직 (가장 높은 우선순위부터 체크)
        // 사망 상태로의 전이는 항상 최우선으로 처리
        if (Context.Stats.CurrentHp <= 0 && !IsCurrentState(UnitState.DEATH)) // CurrentState 대신 IsCurrentState 사용
        {
            ChangeState(UnitState.DEATH);
            return;
        }

        // 타겟이 있는지 여부와 거리 기반 상태 전이
        if (Context.HasTarget())
        {
            if (Context.IsTargetInAttackRange())
            {
                if (!IsCurrentState(UnitState.ATTACK)) 
                {
                    ChangeState(UnitState.ATTACK);
                }
            }
            else if (Context.IsTargetInChaseRange())
            {
                if (!IsCurrentState(UnitState.MOVE)) 
                {
                    ChangeState(UnitState.MOVE);
                }
            }
            else // 타겟은 있지만 추적/공격 범위 밖이라면
            {
                if (!IsCurrentState(UnitState.IDLE)) 
                {
                    ChangeState(UnitState.IDLE);
                }
            }
        }
        else // 타겟을 잃었을 경우
        {
            if (!IsCurrentState(UnitState.IDLE))
            {
                ChangeState(UnitState.IDLE);
            }
            else
            {

            }
        }

        // 현재 상태의 Action을 매 프레임 호출 (해당 상태의 주기적 로직 실행)
        CurrentState?.Action(); // Base 클래스의 CurrentState 사용
    }

    // 이전 상태로 되돌리기 (StateContext에 이미 구현되어 있지만, TEnum을 반환해야 하므로 매핑 필요)
    public void RevertToPreviousState()
    {
        IState<Unit> prev = GetPreviousState();
        if (prev != null && prev != CurrentState)
        {
            // GetStateFromIState 메서드를 사용하여 이전 상태의 TEnum 값을 가져옵니다.
            UnitState previousUnitState = GetStateFromIState(prev);
            if (previousUnitState != default(UnitState)) // 유효한 상태일 경우
            {
                ChangeState(previousUnitState);
            }
            else
            {
                Debug.LogWarning("Cannot revert to previous state: Previous state enum not found.");
            }
        }
    }
}