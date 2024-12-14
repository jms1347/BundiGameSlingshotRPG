using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : MonoBehaviour, IState
{
    [Header("이닛 상태에 필요한 변수")]
    private PlayerController playerController;
    public void Handle(PlayerController pPlayerController)
    {
        if (playerController == null)
        {
            playerController = pPlayerController;
        }
    }
    public void Enter()
    {
        //Debug.Log("초기화 상태 시작");
    }
    public void Action()
    {
        //playerController.Rigid.linearVelocity = Vector3.zero;
    }
    public void Exit()
    {
        //Debug.Log("초기화 상태 끝");
    }
}
