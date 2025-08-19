using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private ICharacter owner;

    public PassiveSkill(ICharacter owner, string name, string description) : base(name, description)
    {
        this.owner = owner;
    }

    public override void Execute()
    {
        // 펫 모드일 때만 호출되므로, 여기에 패시브 효과를 적용합니다.
        // 예를 들어, 주 캐릭터의 공격력을 5% 증가
        Debug.Log($"{owner.Name}'s Passive Skill: {SkillName} is active. (e.g., Increase main character's attack)");
        // 실제 게임 로직에서는 CombatManager나 CharacterManager 등에서
        // 이 패시브 스킬의 효과를 받아서 주 캐릭터의 능력치를 조정합니다.
    }
}