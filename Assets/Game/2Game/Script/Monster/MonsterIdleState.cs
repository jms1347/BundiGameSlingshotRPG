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

        //// ���Ͱ� ������ �� �̻� ���� ������ �������� ����
        //while (monster != null && !monster.IsDead())
        //{
        //    if (monster.HasTarget()) // Ÿ���� �ִ��� ���� Ȯ��
        //    {
        //        if (monster.IsTargetInAttackRange())
        //        {
        //            monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
        //            yield break; // ���� ���� �� ���� �ڷ�ƾ ����
        //        }
        //        else if (monster.IsTargetInChaseRange())
        //        {
        //            monster.monsterStateManager.ChangeState(MonsterState.MOVE);
        //            yield break; // ���� ���� �� ���� �ڷ�ƾ ����
        //        }
        //    }
        //    else // Ÿ���� �Ҿ��� ���
        //    {
        //        Debug.Log("Target lost, returning to IDLE from MoveState.");
        //        monster.monsterStateManager.ChangeState(MonsterState.IDLE);
        //        yield break; // �ڷ�ƾ ����
        //    }

        //    // Ÿ���� ���ų�, �����Ÿ� �ۿ� �ִٸ� ��� IDLE
        //    // Debug.Log("Monster is idling, waiting for target...");

        //    // ���� �����ӱ��� ��ٸ��ų�, Ư�� �ð� �������� üũ (���� ����ȭ)
        //    // yield return null; // �� ������ üũ
        //    yield return Utils.WaitForSecond(0.5f); // 0.5�ʸ��� üũ
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
