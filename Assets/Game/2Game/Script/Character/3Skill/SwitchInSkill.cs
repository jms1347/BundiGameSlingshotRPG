using UnityEngine;

public class SwitchInSkill : BaseSkill
{
    private ICharacter owner;

    public SwitchInSkill(ICharacter owner, string name, string description) : base(name, description)
    {
        this.owner = owner;
    }

    public override void Execute()
    {
        Debug.Log($"{owner.Name} uses Switch In Skill: {SkillName}");
        // 여기에 등장 스킬의 실제 로직 구현 (예: 주변 아군에게 실드 부여)
    }
}