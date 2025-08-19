using System.Collections;
using UnityEngine;

public class MonsterIdleState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Enter()
    {
    }

    public void Action()
    {
        //if (actionCour != null)
        //    StopCoroutine(actionCour);
        //actionCour = ActionCour();
        //StartCoroutine(actionCour);
    }

    IEnumerator ActionCour()
    {
        yield return null;

        //// 몬스터가 죽으면 더 이상 상태 로직을 수행하지 않음
        //while (monster != null && !monster.IsDead())
        //{
        //    if (monster.HasTarget()) // 타겟이 있는지 먼저 확인
        //    {
        //        if (monster.IsTargetInAttackRange())
        //        {
        //            monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
        //            yield break; // 상태 변경 시 현재 코루틴 종료
        //        }
        //        else if (monster.IsTargetInChaseRange())
        //        {
        //            monster.monsterStateManager.ChangeState(MonsterState.MOVE);
        //            yield break; // 상태 변경 시 현재 코루틴 종료
        //        }
        //    }
        //    else // 타겟을 잃었을 경우
        //    {
        //        Debug.Log("Target lost, returning to IDLE from MoveState.");
        //        monster.monsterStateManager.ChangeState(MonsterState.IDLE);
        //        yield break; // 코루틴 종료
        //    }

        //    // 타겟이 없거나, 사정거리 밖에 있다면 계속 IDLE
        //    // Debug.Log("Monster is idling, waiting for target...");

        //    // 다음 프레임까지 기다리거나, 특정 시간 간격으로 체크 (성능 최적화)
        //    // yield return null; // 매 프레임 체크
        //    yield return Utils.WaitForSecond(0.5f); // 0.5초마다 체크
        //}
    }



    public void Exit()
    {        
        //if (actionCour != null)
        //    StopCoroutine(actionCour);
    }

    public void Handle(Monster context)
    {
        if (monster == null)
        {
            monster = context;
        }
    }
}
