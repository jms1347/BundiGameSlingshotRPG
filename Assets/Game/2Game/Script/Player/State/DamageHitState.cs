//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DamageHitState : MonoBehaviour, IState
//{
//    [Header("�ǰ� ���¿� �ʿ��� ����")]
//    private float damage;
    
//    private IEnumerator actionCour;
//    private PlayerController playerController;
//    private PlayerCollController playerCollController;
//    public void Handle(PlayerController pPlayerController)
//    {
//        if (playerController == null)
//        {
//            playerController = pPlayerController;
//            playerCollController = playerController.GetComponent<PlayerCollController>();
//        }
//    }
//    public void Enter()
//    {
//        playerController.RedScreenFadeEffect();

//        playerController.Rigid.linearVelocity = Vector2.zero;

//    }
//    public void Action()
//    {
//        if (actionCour != null)
//            StopCoroutine(actionCour);
//        actionCour = ActionCour();
//        StartCoroutine(actionCour);
//    }

//    IEnumerator ActionCour()
//    {
//        yield return new WaitUntil(() => playerController != null);

//        while (true)
//        {
//            if (playerController.IsGrounded() && playerController.DirX == 0)
//            {
//                playerController.ChangeState(PlayerStateContext.PlayerState.IDLE);
//                break;
//            }

//            yield return null;

//        }
//    }



//    public void Exit()
//    {
//        if (actionCour != null)
//            StopCoroutine(actionCour);
//    }


//}
