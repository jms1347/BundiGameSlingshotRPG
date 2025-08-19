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
        // ���⿡ ���� ��ų�� ���� ���� ���� (��: �ֺ� �Ʊ����� �ǵ� �ο�)
    }
}