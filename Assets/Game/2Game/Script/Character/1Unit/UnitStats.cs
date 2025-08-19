using System;
using UnityEngine;

[System.Serializable] // 인스펙터에서 편집 가능하도록
public class UnitStats
{
    [Header("바 관련")]
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    [Header("써치 관련")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;


    [Header("공격/방어 관련")]
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float defenseArmor;

    [Header("이동속도 관련")]
    [SerializeField] private float moveSpeed;

    //[Header("콜백 관련")]
    public event Action<float, float> OnHealthChanged;


    // ... 기타 스탯

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