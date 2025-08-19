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

        // �ǰ� �ִϸ��̼� ��� (Ʈ���� �Ǵ� bool)
        monster.MonsterAni?.SetTrigger("3_Damaged");

    }
    private IEnumerator ActionCour()
    {
        // �ǰ� ���� �ð� ���� ���
        yield return Utils.WaitForSecond(1f);

        // ���Ͱ� ���� �ʾҴٸ� ���� ���·� �����ϰų� ������ ���� ���·� ��ȯ
        if (!monster.IsDead())
        {
            // �Ϲ������� �ǰ� �Ŀ��� ���� ���·� ���ư��� ��찡 ����
            // Ȥ�� �ٷ� ���� �ൿ�� ������ ���� ����
            monster.monsterStateManager.RevertToPreviousState();
            // �Ǵ�:
            // if (monster.IsTargetInAttackRange()) monster.monsterStateManager.ChangeState(MonsterState.ATTACK);
            // else if (monster.IsTargetInChaseRange()) monster.monsterStateManager.ChangeState(MonsterState.MOVE);
            // else monster.monsterStateManager.ChangeState(MonsterState.IDLE);
        }
        // �׾��ٸ� DEATH ���·��� ��ȯ�� Monster.CurrentHP setter���� �̹� ó����
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