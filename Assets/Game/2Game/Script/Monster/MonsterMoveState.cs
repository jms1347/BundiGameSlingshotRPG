using System.Collections;
using UnityEngine;

public class MonsterMoveState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Enter()
    {
        // ���Ͱ� ���ų� ���� ���¸�, �� ���� ���� ������ �������� �ʽ��ϴ�.
        if (monster == null || monster.IsDead()) return;

        // "Jump" �ִϸ��̼� �Ķ���͸� true�� �����մϴ�.
        // ������ �ִϸ����� ��Ʈ�ѷ��� "Jump"��� bool �Ķ���Ͱ� �־�� �մϴ�.
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
        // ���Ͱ� �װų� ������ �������� ��ƾ ����
        while (monster != null && !monster.IsDead())
        {
            // Ÿ���� �ִ��� Ȯ��
            if (monster.HasTarget())
            {
                // Monster Ŭ������ MoveTowards �޼��带 ����Ͽ� Ÿ���� �Ĵٺ��� �̵��մϴ�.
                // �� �޼��� �ȿ��� ������ ȸ���� ��ġ �̵��� ó���˴ϴ�.
                monster.MoveTowards(monster.playerTarget.position);

                // ���� ������ ���Դ��� üũ
                if (monster.IsTargetInAttackRange())
                {
                    monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
                    yield break; // ���� ���� �� ���� �ڷ�ƾ ����
                }
                // �߰� ������ ������� üũ (�ٽ� Idle ���·� ���ư��ų�, ���� ��ġ�� ����)
                else if (!monster.IsTargetInChaseRange())
                {
                    monster.monsterStateManager.ChangeState(MonsterState.IDLE);
                    yield break; // ���� ���� �� ���� �ڷ�ƾ ����
                }
            }
            else // Ÿ���� �Ҿ��� ���
            {
                Debug.Log("Target lost, returning to IDLE from MoveState.");
                monster.monsterStateManager.ChangeState(MonsterState.IDLE);
                yield break; // �ڷ�ƾ ����
            }

            // �� ������ �����ϵ��� yield return null ���
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
