using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //Player 컨트롤러 세팅
    void Handle(PlayerController pPlayerController);

    //상태 변경 되었을 때 1회 호출
    void Enter();

    //상태 변경 후 update
    void Action();

    //다른 상태로 변경 직전 1회 호출
    void Exit();
}
