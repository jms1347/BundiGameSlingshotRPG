using UnityEngine;
using System;

[System.Serializable]
public class UnitStats
{
    [Header("�� ����")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;

    [Header("��ġ ����")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("����/��� ����")]
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float defenseArmor;

    [Header("�̵��ӵ� ����")]
    [SerializeField] private float moveSpeed;

    // ������Ƽ
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, maxHp);
    }

    public float MaxMp { get => maxMp; set => maxMp = value; }
    public float CurrentMp
    {
        get => currentMp;
        set => currentMp = Mathf.Clamp(value, 0, maxMp);
    }

    public float AttackPower { get => attackPower; set => attackPower = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ChaseRange { get => chaseRange; set => chaseRange = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float DefenseArmor { get => defenseArmor; set => defenseArmor = value; }

    public UnitStats(float maxHp, float maxMp, float chaseRange, float attackRange,
        float attackPower, float attackSpeed, float defenseArmor, float moveSpeed)
    {
        MaxHp = maxHp;
        CurrentHp = maxHp;
        MaxMp = maxMp;
        CurrentMp = maxMp;
        ChaseRange = chaseRange;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackSpeed = attackSpeed;
        DefenseArmor = defenseArmor;
        MoveSpeed = moveSpeed;
    }

    // �ʱ�ȭ �޼���
    public void InitStats()
    {
        CurrentHp = maxHp;
        CurrentMp = maxMp;
    }

    // ������ �Դ� �޼���
    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }

    // ȸ�� �޼���
    public void Heal(float healAmount)
    {
        CurrentHp += healAmount;
    }

    // ���� ��� �޼���
    public void UseMana(float manaCost)
    {
        CurrentMp -= manaCost;
    }

    // ���� ȸ�� �޼���
    public void ManaRegen(float regenAmount)
    {
        CurrentMp += regenAmount;
    }

    // ��� ���� üũ
    public bool IsDead()
    {
        return CurrentHp <= 0;
    }
}