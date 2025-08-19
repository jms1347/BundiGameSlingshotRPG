//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerStateContext : MonoBehaviour
//{
//    public enum PlayerState
//    {
//        IDLE = 0,
//        MOVE = 1,
//        JUMP = 2,
//        DamageHit = 4,
//        Death = 99
//    }
//    private Dictionary<PlayerState, IState> statePool;
//    private IState currentState;
//    private IState previousState; // ���� ���¸� ������ ����
//    private PlayerController playerController;
//    public IState CurrentState { get => currentState; set => currentState = value; }
//    public Dictionary<PlayerState, IState> StatePool { get => statePool; set => statePool = value; }

//    public void SettingPlayerController(PlayerController pPlayerController)
//    {
//        playerController = pPlayerController;
//    }
//    private void Awake()
//    {
//        InitStatePool();
//    }

//    public void InitStatePool()
//    {
//        StatePool = new Dictionary<PlayerState, IState>();
//        StatePool[PlayerState.IDLE] = gameObject.AddComponent<IdleState>();
//        StatePool[PlayerState.MOVE] = gameObject.AddComponent<MoveState>();
//        StatePool[PlayerState.JUMP] = gameObject.AddComponent<JumpState>();
//        StatePool[PlayerState.DamageHit] = gameObject.AddComponent<DamageHitState>();
//        StatePool[PlayerState.Death] = gameObject.AddComponent<DeathState>();

//        foreach(var i in StatePool)
//        {
//            StatePool[i.Key]?.Handle(playerController);
//        }
//    }

//    public void Transition(PlayerState pType)
//    {
//        Debug.Log("������� : " + pType.ToString()) ;
//        CurrentState?.Exit();
//        previousState = CurrentState; // ���� ���¸� ���� ���·� ����

//        CurrentState = StatePool[pType];
//        //CurrentState?.Handle(playerController);
//        CurrentState?.Enter();
//        CurrentState?.Action();
//    }

//    public PlayerState GetPlayerStateFromIState(IState state)
//    {
//        foreach (var kvp in StatePool)
//        {
//            if (kvp.Value == state)
//            {
//                return kvp.Key;
//            }
//        }
//        return PlayerState.IDLE; // �⺻ ���·� ���ư���
//    }

//    public bool IsCurrentState(PlayerState pState)
//    {
//        if(currentState == StatePool[pState])
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//}
