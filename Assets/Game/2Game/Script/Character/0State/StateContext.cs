using System.Collections.Generic;
using UnityEngine;

public class StateContext<T, TEnum> : MonoBehaviour where T : MonoBehaviour where TEnum : System.Enum
{
    private Dictionary<TEnum, IState<T>> statePool;
    private IState<T> currentState;
    private IState<T> previousState;
    private T context;

    public IState<T> CurrentState => currentState;
    public Dictionary<TEnum, IState<T>> StatePool => statePool;
    public T Context => context; // �� �κ��� �ٽ�!


    public void SettingContext(T pContext)
    {
        context = pContext;
    }

    private void Awake()
    {
        //InitStatePool();
    }

    private void Start()
    {
        InitStatePool();

    }

    protected virtual void InitStatePool()
    {
        statePool = new Dictionary<TEnum, IState<T>>();
        // �� ���¸� statePool�� �߰��ϴ� ������ �ڽ� Ŭ�������� ����
    }

    public void ChangeState(TEnum pType)
    {
        Debug.Log($"{context.gameObject.name} State Transition: {pType}");
        currentState?.Exit();
        previousState = currentState;
        currentState = statePool[pType];
        currentState?.Handle(context);
        currentState?.Enter();
        //currentState?.Action(); // Enter �� �ٷ� Action�� �������� ���δ� ��Ȳ�� ���� ����// ������Ʈ�� ����

    }

    public TEnum GetStateFromIState(IState<T> state)
    {
        foreach (var kvp in StatePool)
        {
            if (kvp.Value.Equals(state)) // ���� ��ü ��
            {
                return kvp.Key;
            }
        }
        return default(TEnum); // �⺻�� ��ȯ
    }

    public bool IsCurrentState(TEnum pState)
    {
        return currentState.Equals(StatePool[pState]); // ���� ��ü ��
    }

    public IState<T> GetPreviousState()
    {
        return previousState;
    }
}