using UnityEngine;
using System.Collections.Generic;

public interface ICharacter
{
    string Name { get; }
    int CurrentHP { get; set; }
    int MaxHP { get; }
    int CurrentMP { get; set; }
    int MaxMP { get; }
    List<ISkill> Skills { get; } // 모든 스킬을 담을 리스트

    void TakeDamage(int amount);
    void Heal(int amount);
    void GainMP(int amount);
    void UseMP(int amount);
    void ActivatePassiveSkills(); // 펫 모드일 때 패시브 스킬 활성화
    void UseActiveSkill(); // 마나 꽉 찼을 때 자동 발동 스킬
    void UseUltimateSkill(); // 궁극기 스킬
    void OnSwitchOutSkill(); // 교체될 때 사용하는 스킬
    void OnSwitchInSkill(); // 교체되면서 등장할 때 사용하는 스킬
}