// UnitStateManager.cs
using UnityEngine;
using System.Collections.Generic;

// IState �������̽��� Unit Ÿ������ ���ǵǾ�� �մϴ�.
// public interface IState<T> where T : MonoBehaviour { ... }

// UnitState Enum�� Unit.cs �Ǵ� ������ GlobalEnums.cs�� ���ǵǾ�� �մϴ�.
// public enum UnitState { IDLE, MOVE, ATTACK, HIT, DEATH, ... }

public class UnitStateManager : StateContext<Unit, UnitState>
{
    protected override void InitStatePool()
    {
        base.InitStatePool(); // �θ� Ŭ������ Dictionary �ʱ�ȭ ȣ��

        // ���⿡ Unit�� ��ü���� ���µ��� statePool�� �߰��մϴ�.
        // �� ���� Ŭ������ IState<Unit>�� �����ؾ� �մϴ�.
        // GetOrAddComponent ���� �޼��带 �߰��Ͽ� �����ϰ� ó���� �� �ֽ��ϴ�.
        StatePool[UnitState.IDLE] = GetOrAddComponent<UnitIdleState>();
        StatePool[UnitState.MOVE] = GetOrAddComponent<UnitMoveState>();
        StatePool[UnitState.ATTACK] = GetOrAddComponent<UnitAttackState>();
        StatePool[UnitState.DAMAGEHIT] = GetOrAddComponent<UnitDamageHitState>();
        StatePool[UnitState.DEATH] = GetOrAddComponent<UnitDeathState>();

        // �� ���¿� context(Unit)�� �����Ͽ� �ʱ�ȭ�մϴ�.
        foreach (var i in StatePool)
        {            
            i.Value?.Handle(Context); 
        }

        ChangeState(UnitState.IDLE);

    }

    // GetComponent<T>() �Ǵ� AddComponent<T>()�� �����ϰ� ó���ϴ� ���� �޼���
    private TState GetOrAddComponent<TState>() where TState : Component, IState<Unit> // IState<Unit> Ÿ������ ����
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

        // 1. ���� ���� ���� ���� (���� ���� �켱�������� üũ)
        // ��� ���·��� ���̴� �׻� �ֿ켱���� ó��
        if (Context.Stats.CurrentHp <= 0 && !IsCurrentState(UnitState.DEATH)) // CurrentState ��� IsCurrentState ���
        {
            ChangeState(UnitState.DEATH);
            return;
        }

        // Ÿ���� �ִ��� ���ο� �Ÿ� ��� ���� ����
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
            else // Ÿ���� ������ ����/���� ���� ���̶��
            {
                if (!IsCurrentState(UnitState.IDLE)) 
                {
                    ChangeState(UnitState.IDLE);
                }
            }
        }
        else // Ÿ���� �Ҿ��� ���
        {
            if (!IsCurrentState(UnitState.IDLE))
            {
                ChangeState(UnitState.IDLE);
            }
            else
            {

            }
        }

        // ���� ������ Action�� �� ������ ȣ�� (�ش� ������ �ֱ��� ���� ����)
        CurrentState?.Action(); // Base Ŭ������ CurrentState ���
    }

    // ���� ���·� �ǵ����� (StateContext�� �̹� �����Ǿ� ������, TEnum�� ��ȯ�ؾ� �ϹǷ� ���� �ʿ�)
    public void RevertToPreviousState()
    {
        IState<Unit> prev = GetPreviousState();
        if (prev != null && prev != CurrentState)
        {
            // GetStateFromIState �޼��带 ����Ͽ� ���� ������ TEnum ���� �����ɴϴ�.
            UnitState previousUnitState = GetStateFromIState(prev);
            if (previousUnitState != default(UnitState)) // ��ȿ�� ������ ���
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