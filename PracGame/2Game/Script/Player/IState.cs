using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //Player ��Ʈ�ѷ� ����
    void Handle(PlayerController pPlayerController);

    //���� ���� �Ǿ��� �� 1ȸ ȣ��
    void Enter();

    //���� ���� �� update
    void Action();

    //�ٸ� ���·� ���� ���� 1ȸ ȣ��
    void Exit();
}
