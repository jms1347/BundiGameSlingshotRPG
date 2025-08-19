using UnityEngine;
using System.Collections.Generic;

public interface ICharacter
{
    string Name { get; }
    int CurrentHP { get; set; }
    int MaxHP { get; }
    int CurrentMP { get; set; }
    int MaxMP { get; }
    List<ISkill> Skills { get; } // ��� ��ų�� ���� ����Ʈ

    void TakeDamage(int amount);
    void Heal(int amount);
    void GainMP(int amount);
    void UseMP(int amount);
    void ActivatePassiveSkills(); // �� ����� �� �нú� ��ų Ȱ��ȭ
    void UseActiveSkill(); // ���� �� á�� �� �ڵ� �ߵ� ��ų
    void UseUltimateSkill(); // �ñر� ��ų
    void OnSwitchOutSkill(); // ��ü�� �� ����ϴ� ��ų
    void OnSwitchInSkill(); // ��ü�Ǹ鼭 ������ �� ����ϴ� ��ų
}