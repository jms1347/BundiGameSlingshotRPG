using UnityEngine;
using System.Collections.Generic;

public abstract class BaseCharacter : MonoBehaviour, ICharacter
{
    [Header("Character Stats")]
    [SerializeField] protected string characterName;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int maxMP;

    protected int currentHP;
    protected int currentMP;

    public string Name => characterName;
    public int CurrentHP
    {
        get => currentHP;
        set => currentHP = Mathf.Clamp(value, 0, MaxHP);
    }
    public int MaxHP => maxHP;
    public int CurrentMP
    {
        get => currentMP;
        set => currentMP = Mathf.Clamp(value, 0, MaxMP);
    }
    public int MaxMP => maxMP;

    // ��ų ����� �ڽ� Ŭ�������� �ʱ�ȭ�ϵ��� abstract�� ����
    public abstract List<ISkill> Skills { get; }

    protected virtual void Awake()
    {
        currentHP = maxHP;
        currentMP = 0; // ������ 0���� ����
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        Debug.Log($"{Name} took {amount} damage. Current HP: {CurrentHP}");
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;
        Debug.Log($"{Name} healed {amount}. Current HP: {CurrentHP}");
    }

    public void GainMP(int amount)
    {
        CurrentMP += amount;
        Debug.Log($"{Name} gained {amount} MP. Current MP: {CurrentMP}");
        if (CurrentMP >= MaxMP)
        {
            UseActiveSkill(); // ������ �� á�� �� ��Ƽ�� ��ų �ڵ� �ߵ�
            CurrentMP = 0; // ��ų ��� �� ���� �ʱ�ȭ
        }
    }

    public void UseMP(int amount)
    {
        CurrentMP -= amount;
        Debug.Log($"{Name} used {amount} MP. Current MP: {CurrentMP}");
    }

    // �Ʒ� ��ų ���� �޼������ �������̽����� ����������,
    // BaseCharacter���� �� ��ų Ÿ�Կ� �´� ��ų�� ã�� �����ϴ� ������� ������ �� �ֽ��ϴ�.
    // �Ǵ�, �� ĳ���Ͱ� ���� �ڽ��� ��ų�� �����ϵ��� �߻� �޼���� ���ܵ� ���� �ֽ��ϴ�.
    // ���⼭�� �⺻������ ��ų ��Ͽ��� ã�� �����ϴ� ������� �����մϴ�.

    public virtual void ActivatePassiveSkills()
    {
        foreach (var skill in Skills)
        {
            if (skill is PassiveSkill passiveSkill)
            {
                passiveSkill.Execute();
            }
        }
    }

    public virtual void UseActiveSkill()
    {
        foreach (var skill in Skills)
        {
            if (skill is ActiveSkill activeSkill)
            {
                // ��Ƽ�� ��ų�� ������ á�� �� �ڵ����� �ߵ��ϴ� ��ų�̹Ƿ�,
                // ���⼭�� Ư�� ���� ���� �ٷ� �����մϴ�.
                activeSkill.Execute();
                break; // �ϳ��� ��Ƽ�� ��ų�� �ִٰ� ����
            }
        }
    }

    public virtual void UseUltimateSkill()
    {
        foreach (var skill in Skills)
        {
            if (skill is UltimateSkill ultimateSkill)
            {
                if (ultimateSkill.IsReady())
                {
                    ultimateSkill.Execute();
                }
                else
                {
                    Debug.Log($"{Name}'s ultimate skill is on cooldown.");
                }
                break; // �ϳ��� �ñر� ��ų�� �ִٰ� ����
            }
        }
    }

    public virtual void OnSwitchOutSkill()
    {
        foreach (var skill in Skills)
        {
            if (skill is SwitchOutSkill switchOutSkill)
            {
                switchOutSkill.Execute();
                break; // �ϳ��� ��ü �� ���� ��ų�� �ִٰ� ����
            }
        }
    }

    public virtual void OnSwitchInSkill()
    {
        foreach (var skill in Skills)
        {
            if (skill is SwitchInSkill switchInSkill)
            {
                switchInSkill.Execute();
                break; // �ϳ��� ��ü �� ���� ��ų�� �ִٰ� ����
            }
        }
    }

    protected abstract void Die(); // ���� ó�� ������ ĳ���͸��� �ٸ� �� �����Ƿ� �߻� �޼����
}