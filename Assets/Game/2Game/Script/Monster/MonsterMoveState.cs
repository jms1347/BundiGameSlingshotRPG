using System.Collections;
using UnityEngine;

public class MonsterMoveState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Enter()
    {
        // 몬스터가 없거나 죽은 상태면, 이 상태 진입 로직을 실행하지 않습니다.
        if (monster == null || monster.IsDead()) return;

        // "Jump" 애니메이션 파라미터를 true로 설정합니다.
        // 몬스터의 애니메이터 컨트롤러에 "Jump"라는 bool 파라미터가 있어야 합니다.
        monster.MonsterAni?.SetBool("1_Move", true);
    }
    public void Action()
    {
        if (monster == null || monster.IsDead()) return;

        if (actionCour != null)
            StopCoroutine(actionCour);
        actionCour = ActionCour();
        StartCoroutine(actionCour);
    }

    IEnumerator ActionCour()
    {
        // 몬스터가 죽거나 참조가 없어지면 루틴 종료
        while (monster != null && !monster.IsDead())
        {
            // 타겟이 있는지 확인
            if (monster.HasTarget())
            {
                // Monster 클래스의 MoveTowards 메서드를 사용하여 타겟을 쳐다보고 이동합니다.
                // 이 메서드 안에서 몬스터의 회전과 위치 이동이 처리됩니다.
                monster.MoveTowards(monster.playerTarget.position);

                // 공격 범위에 들어왔는지 체크
                if (monster.IsTargetInAttackRange())
                {
                    monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
                    yield break; // 상태 변경 시 현재 코루틴 종료
                }
                // 추격 범위를 벗어났는지 체크 (다시 Idle 상태로 돌아가거나, 원래 위치로 복귀)
                else if (!monster.IsTargetInChaseRange())
                {
                    monster.monsterStateManager.ChangeState(MonsterState.IDLE);
                    yield break; // 상태 변경 시 현재 코루틴 종료
                }
            }
            else // 타겟을 잃었을 경우
            {
                Debug.Log("Target lost, returning to IDLE from MoveState.");
                monster.monsterStateManager.ChangeState(MonsterState.IDLE);
                yield break; // 코루틴 종료
            }

            // 매 프레임 실행하도록 yield return null 사용
            yield return null;
        }
    }



    public void Exit()
    {
        if (actionCour != null)
            StopCoroutine(actionCour);
        monster.MonsterAni.SetBool("1_Move", false);
    }

    public void Handle(Monster context)
    {
        if (monster == null)
        {
            monster = context;
        }
    }
}
