using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // �� ��ų�� ����ϴ� ĳ���� ����
    private ICharacter owner;

    public ActiveSkill(ICharacter owner, string name, string description) : base(name, description)
    {
        this.owner = owner;
    }

    public override void Execute()
    {
        Debug.Log($"{owner.Name} uses Active Skill: {SkillName}");
        // ���⿡ ��Ƽ�� ��ų�� ���� ���� ���� (��: Ư�� ������ ������)
        // ���� ���, �ֺ� ������ 10 ������
        // CombatManager.Instance.DealDamageToEnemies(10);
    }
}