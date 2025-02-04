using UnityEngine;

public class DefaultAttackBehavior : SpecialAttackBehavior
{
    public override void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force)
    {
        // �⺻ Ư�� ����: ���� ���, ���� ������ �̵��� �����ϰ� �÷��̾�� ���� ����
        Rigidbody rb = origin != null ? null : null; // �����δ� �÷��̾��� Rigidbody�� �����ؾ� ��.
        // ���⼭�� ������ Debug.Log�� ǥ���մϴ�.
        Debug.Log("Default Attack executed. Force: " + force);
        // �ʿ信 ���� �߰� ȿ��(����Ʈ ��)�� �����ϼ���.
    }
}