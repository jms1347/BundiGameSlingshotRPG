using System.Collections;
using UnityEngine;


public class MonsterDamageHitState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Action()
    {
        if (monster == null || monster.IsDead()) return;

        if (actionCour != null)
            StopCoroutine(actionCour);
        actionCour = ActionCour();
        StartCoroutine(actionCour);
    }

    public void Enter()
    {
        if (monster == null || monster.IsDead()) return;

        // 피격 애니메이션 재생 (트리거 또는 bool)
        monster.MonsterAni?.SetTrigger("3_Damaged");

    }
    private IEnumerator ActionCour()
    {
        // 피격 경직 시간 동안 대기
        yield return Utils.WaitForSecond(1f);

        // 몬스터가 죽지 않았다면 이전 상태로 복귀하거나 적절한 다음 상태로 전환
        if (!monster.IsDead())
        {
            // 일반적으로 피격 후에는 이전 상태로 돌아가는 경우가 많음
            // 혹은 바로 다음 행동을 결정할 수도 있음
            monster.monsterStateManager.RevertToPreviousState();
            // 또는:
            // if (monster.IsTargetInAttackRange()) monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
            // else if (monster.IsTargetInChaseRange()) monster.monsterStateManager.ChangeState(MonsterState.MOVE);
            // else monster.monsterStateManager.ChangeState(MonsterState.IDLE);
        }
        // 죽었다면 DEATH 상태로의 전환은 Monster.CurrentHP setter에서 이미 처리됨
        Debug.Log("HitRoutine terminated.");
    }
    public void Exit()
    {
        if (actionCour != null)
            StopCoroutine(actionCour);
        monster.MonsterAni.SetBool("Damage", false);
    }

    public void Handle(Monster context)
    {
        if (monster == null)
        {
            monster = context;
        }
    }
}