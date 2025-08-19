using UnityEngine;

public class UltimateSkill : BaseSkill
{
    private ICharacter owner;
    private float cooldownTime; // ��Ÿ��
    private float lastUsedTime; // ������ ��� �ð�

    public UltimateSkill(ICharacter owner, string name, string description, float cooldown) : base(name, description)
    {
        this.owner = owner;
        cooldownTime = cooldown;
        lastUsedTime = -cooldown; // �ʱ⿡�� �ٷ� ��� �����ϵ���
    }

    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldownTime;
    }

    public override void Execute()
    {
        if (IsReady())
        {
            Debug.Log($"{owner.Name} uses Ultimate Skill: {SkillName}");
            // ���⿡ �ñر� ��ų�� ���� ���� ���� (��: ���� ������, ����)
            lastUsedTime = Time.time;
        }
        else
        {
            Debug.Log($"{SkillName} is on cooldown. Remaining: {cooldownTime - (Time.time - lastUsedTime):F2}s");
        }
    }
}