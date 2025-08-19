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
        // �� ����� ���� ȣ��ǹǷ�, ���⿡ �нú� ȿ���� �����մϴ�.
        // ���� ���, �� ĳ������ ���ݷ��� 5% ����
        Debug.Log($"{owner.Name}'s Passive Skill: {SkillName} is active. (e.g., Increase main character's attack)");
        // ���� ���� ���������� CombatManager�� CharacterManager ���
        // �� �нú� ��ų�� ȿ���� �޾Ƽ� �� ĳ������ �ɷ�ġ�� �����մϴ�.
    }
}