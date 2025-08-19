//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class JumpState : MonoBehaviour, IState
//{
//    [Header("���� ���¿� �ʿ��� ����")]
//    private Vector2 vecGravity;
//     private float jumpTime;
//    private float jumpCounter;
//    private float jumpMultiplier;

//    private PlayerController playerController;
//    private IEnumerator actionCour;

//    public void Handle(PlayerController pPlayerController)
//    {
//        if (playerController == null)
//        {
//            playerController = pPlayerController;
//            vecGravity = new Vector2(0, Physics2D.gravity.y);
//            jumpCounter = playerController.JumpCounter;
//            jumpTime = playerController.JumpTime;
//            jumpMultiplier = playerController.JumpMultiplier;
//        }
//    }

//    public void Enter()
//    {        
//       // playerController.OnJumpEnter();

//        Jump();
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
//            JumpFromUpdate();

//            if (playerController.Rigid.linearVelocity.y < 0)
//            {
//               // playerController.ChangeState(PlayerStateContext.PlayerState.DROP);
//                break;
//            }
//            yield return null;
//        }
//    }
//    public void JumpFromUpdate()
//    {
//        if (playerController.Rigid.linearVelocity.y > 0 && playerController.IsJumping)
//        {
//            jumpCounter += Time.deltaTime;
//            if (jumpCounter > jumpTime)
//            {
//                playerController.IsJumping = false;
//            }

//            float t = jumpCounter / jumpTime;
//            float currentJumpM = jumpMultiplier;

//            if (t > 0.5f)
//            {
//                currentJumpM = jumpMultiplier * (1 - t);
//            }

//            playerController.Rigid.linearVelocity += vecGravity * currentJumpM * Time.deltaTime;
//        }
//    }

//    public void Jump()
//    {

//        if (playerController.CurrentJumpCnt < playerController.MaxJumpCnt)
//        {
//           // SoundManager.instance.PlayCreateSfx(SoundManager.SoundType.Sfx, SoundKeyStringUtils.GetSoundKeyString(SoundKeyStringUtils.SoundNameKey.Jump),0.1f);

//            playerController.Ani.Play("PlayerJump");

//            playerController.Rigid.linearVelocity = new Vector2(playerController.Rigid.linearVelocity.x, playerController.JumpPower);
//            //playerController.Rigid.AddForce(new Vector2(0, playerController.JumpPower), ForceMode2D.Impulse);
//            // ���� Ƚ�� 1 ����
//            playerController.CurrentJumpCnt++;
//        }
//    }

//    public void Exit()
//    {
//        playerController.IsJumping = false;
//        if (actionCour != null)
//            StopCoroutine(actionCour);
//    }
//}
