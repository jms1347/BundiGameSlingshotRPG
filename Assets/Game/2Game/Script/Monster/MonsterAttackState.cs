using UnityEngine;
using System.Collections;
public class MonsterAttackState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Action()
    {
        // ���Ͱ� ���ų� ���� ���¸�, �� ���� ���� ������ �������� �ʽ��ϴ�.
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
            // ���� �ִϸ��̼� ��� �ð� ���� ���
            // ���� ����� �ִ� ������ �ִϸ��̼� �̺�Ʈ�� ó���ϴ� ���� �Ϲ���
            monster.PerformAttack(); // �� �޼��� �ȿ��� Ÿ�� ȸ�� ���� �̷����
            monster.MonsterAni?.SetTrigger("2_Attack");

            yield return Utils.WaitForSecond(monster.attackDuration); // ���� �ִϸ��̼� ��� �ð� ���

            // ���Ͱ� ������ ���� ���� ���� �ְ� ���� �ʾҴٸ� �ٽ� ����, �ƴϸ� �ٸ� ���·� ��ȯ
            if (monster.IsTargetInAttackRange() && monster.HasTarget())
            {
                yield return Utils.WaitForSecond(monster.attackCooldown - monster.attackDuration); // ��ٿ� �ð� ���
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
        // ���Ͱ� ���ų� ���� ���¸�, �� ���� ���� ������ �������� �ʽ��ϴ�.
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
