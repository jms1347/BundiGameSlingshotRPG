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
        // ���⿡ ���� ��ų�� ���� ���� ���� (��: ������ ���ο� �����)
    }
}