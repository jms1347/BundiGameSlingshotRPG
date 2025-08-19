using System;
using UnityEngine;

[System.Serializable] // �ν����Ϳ��� ���� �����ϵ���
public class UnitStats
{
    [Header("�� ����")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    [Header("��ġ ����")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;


    [Header("����/��� ����")]
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float defenseArmor;

    [Header("�̵��ӵ� ����")]
    [SerializeField] private float moveSpeed;

    //[Header("�ݹ� ����")]
    public event Action<float, float> OnHealthChanged;


    // ... ��Ÿ ����

    public float MaxHP { get => maxHp; set => maxHp = value; }
    public float CurrentHP
    {
        get => currentHp;
        set
        {
            if (currentHp == value) return;
            currentHp = Mathf.Clamp(value, 0, maxHp);
            OnHealthChanged?.Invoke(currentHp, maxHp);
        }
    }
    public float AttackPower { get => attackPower; set => attackPower = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange
    {
        get => attackRange;
        set
        {
            if (attackRange == value) return;
            attackRange = value;
        }
    }
    public float ChaseRange
    {
        get => chaseRange;
        set
        {
            if (chaseRange == value) return;
            chaseRange = value;
        }
    }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float DefenseArmor { get => defenseArmor; set => defenseArmor = value; }



    public UnitStats(float maxHp, float chaseRange, float attackRange, 
        float attackPower, float attackSpeed, float defenseArmor, float moveSpeed)
    {
        MaxHP = maxHp;
        CurrentHP = MaxHP;
        ChaseRange = chaseRange;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackSpeed = attackSpeed;
        DefenseArmor = defenseArmor;
        MoveSpeed = moveSpeed;        
    }

    public void InitStats()
    {
        CurrentHP = maxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }
}