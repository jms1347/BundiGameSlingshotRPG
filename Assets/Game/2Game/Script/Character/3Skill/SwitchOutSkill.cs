using UnityEngine;


public class SwitchOutSkill : BaseSkill
{
    private ICharacter owner;

    public SwitchOutSkill(ICharacter owner, string name, string description) : base(name, description)
    {
        this.owner = owner;
    }

    public override void Execute()
    {
        Debug.Log($"{owner.Name} uses Switch Out Skill: {SkillName}");
        // 여기에 퇴장 스킬의 실제 로직 구현 (예: 적에게 슬로우 디버프)
    }
}