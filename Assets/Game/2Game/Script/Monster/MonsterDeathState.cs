using System.Collections;
using UnityEngine;



public class MonsterDeathState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Action()
    {
        if (monster == null) return; // �� ���´� ���Ͱ� �׾��� ���� ȣ��ǹǷ� !IsDead() üũ�� �ʿ� ����

        if (actionCour != null)
            StopCoroutine(actionCour);
        actionCour = ActionCour();
        StartCoroutine(actionCour);
    }
    private IEnumerator ActionCour()
    {
        // ��� �ִϸ��̼� ��� �ð���ŭ ���
        yield return Utils.WaitForSecond(3f);
        
        // ��� �ִϸ��̼��� ������ ������Ʈ ��Ȱ��ȭ �Ǵ� Ǯ�� ��ȯ
        // ��: monster.gameObject.SetActive(false);
        Destroy(monster.gameObject); // �׽�Ʈ�� ���� �ٷ� �ı�

        Debug.Log("DeathRoutine terminated.");
    }

    public void Enter()
    {
        if (monster == null) return; // �� ���´� ���Ͱ� �׾��� ���� ȣ��ǹǷ� !IsDead() üũ�� �ʿ� ����

        // ��� �ִϸ��̼� ���
        monster.MonsterAni?.SetTrigger("4_Death");
    }

    public void Exit()
    {
        if (actionCour != null)
            StopCoroutine(actionCour);

    }

    public void Handle(Monster context)
    {
        if (monster == null)
        {
            monster = context;
        }
    }
}
