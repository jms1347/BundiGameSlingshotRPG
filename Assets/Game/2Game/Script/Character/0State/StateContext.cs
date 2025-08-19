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
    public T Context => context; // 이 부분이 핵심!


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
        // 각 상태를 statePool에 추가하는 로직은 자식 클래스에서 구현
    }

    public void ChangeState(TEnum pType)
    {
        Debug.Log($"{context.gameObject.name} State Transition: {pType}");
        currentState?.Exit();
        previousState = currentState;
        currentState = statePool[pType];
        currentState?.Handle(context);
        currentState?.Enter();
        //currentState?.Action(); // Enter 후 바로 Action을 실행할지 여부는 상황에 따라 결정// 업데이트에 넣음

    }

    public TEnum GetStateFromIState(IState<T> state)
    {
        foreach (var kvp in StatePool)
        {
            if (kvp.Value.Equals(state)) // 상태 객체 비교
            {
                return kvp.Key;
            }
        }
        return default(TEnum); // 기본값 반환
    }

    public bool IsCurrentState(TEnum pState)
    {
        return currentState.Equals(StatePool[pState]); // 상태 객체 비교
    }

    public IState<T> GetPreviousState()
    {
        return previousState;
    }
}