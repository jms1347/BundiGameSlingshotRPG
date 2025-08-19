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

    // 스킬 목록은 자식 클래스에서 초기화하도록 abstract로 선언
    public abstract List<ISkill> Skills { get; }

    protected virtual void Awake()
    {
        currentHP = maxHP;
        currentMP = 0; // 마나는 0부터 시작
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
            UseActiveSkill(); // 마나가 다 찼을 때 액티브 스킬 자동 발동
            CurrentMP = 0; // 스킬 사용 후 마나 초기화
        }
    }

    public void UseMP(int amount)
    {
        CurrentMP -= amount;
        Debug.Log($"{Name} used {amount} MP. Current MP: {CurrentMP}");
    }

    // 아래 스킬 관련 메서드들은 인터페이스에서 정의했지만,
    // BaseCharacter에서 각 스킬 타입에 맞는 스킬을 찾아 실행하는 방식으로 구현할 수 있습니다.
    // 또는, 각 캐릭터가 직접 자신의 스킬을 관리하도록 추상 메서드로 남겨둘 수도 있습니다.
    // 여기서는 기본적으로 스킬 목록에서 찾아 실행하는 방식으로 구현합니다.

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
                // 액티브 스킬은 마나가 찼을 때 자동으로 발동하는 스킬이므로,
                // 여기서는 특정 조건 없이 바로 실행합니다.
                activeSkill.Execute();
                break; // 하나의 액티브 스킬만 있다고 가정
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
                break; // 하나의 궁극기 스킬만 있다고 가정
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
                break; // 하나의 교체 시 퇴장 스킬만 있다고 가정
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
                break; // 하나의 교체 시 등장 스킬만 있다고 가정
            }
        }
    }

    protected abstract void Die(); // 죽음 처리 로직은 캐릭터마다 다를 수 있으므로 추상 메서드로
}