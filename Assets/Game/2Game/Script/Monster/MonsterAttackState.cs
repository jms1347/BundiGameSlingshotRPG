using UnityEngine;
using System.Collections;
public class MonsterAttackState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Action()
    {
        // 몬스터가 없거나 죽은 상태면, 이 상태 진입 로직을 실행하지 않습니다.
        if (monster == null || monster.IsDead()) return;

        if (actionCour != null)
            StopCoroutine(actionCour);
        actionCour = ActionCour();
        StartCoroutine(actionCour);
    }
    private IEnumerator ActionCour()
    {
        while (monster != null && !monster.IsDead())
        {
            // 공격 애니메이션 재생 시간 동안 대기
            // 실제 대미지 주는 로직은 애니메이션 이벤트로 처리하는 것이 일반적
            monster.PerformAttack(); // 이 메서드 안에서 타겟 회전 등이 이루어짐
            monster.MonsterAni?.SetTrigger("2_Attack");

            yield return Utils.WaitForSecond(monster.attackDuration); // 공격 애니메이션 재생 시간 대기

            // 몬스터가 여전히 공격 범위 내에 있고 죽지 않았다면 다시 공격, 아니면 다른 상태로 전환
            if (monster.IsTargetInAttackRange() && monster.HasTarget())
            {
                yield return Utils.WaitForSecond(monster.attackCooldown - monster.attackDuration); // 쿨다운 시간 대기
            }
            else if (monster.IsTargetInChaseRange())
            {
                monster.monsterStateManager.ChangeState(MonsterState.MOVE);
                yield break;
            }
            else
            {
                monster.monsterStateManager.ChangeState(MonsterState.IDLE);
                yield break;
            }
        }
        Debug.Log("AttackRoutine terminated.");
    }

public void Enter()
    {        
        // 몬스터가 없거나 죽은 상태면, 이 상태 진입 로직을 실행하지 않습니다.
        if (monster == null || monster.IsDead()) return;


    }

    public void Exit()
    {
        if (actionCour != null)
            StopCoroutine(actionCour);

    }

    public void Handle(Monster context)
    {
        if(monster == null)
        {
            monster = context;
        }
    }
}
