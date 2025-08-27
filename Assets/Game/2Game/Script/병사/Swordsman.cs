using UnityEngine;

public class Swordsman : SoldierBase
{
    [Header("Swordsman Stats")]
    public float defenseMultiplier = 1.2f; // ���� ��� (�⺻ ü�¿� ������)

    protected override void Awake()
    {
        // SoldierBase�� Awake �Լ��� ���� ȣ��
        base.Awake();

        // �˺� ������ �ɷ�ġ ����
        moveSpeed = 2.5f;          // �˺��� �� ���ϹǷ� �̵� �ӵ��� �ټ� ������
        attackSpeed = 1.0f;        // ���� �ӵ��� ������
        attackDamage = 15;         // ���ݷ��� �ؼ��ϰ�
        currentHealth *= defenseMultiplier; // ���� ����� �����Ͽ� ü�� ����
    }
}