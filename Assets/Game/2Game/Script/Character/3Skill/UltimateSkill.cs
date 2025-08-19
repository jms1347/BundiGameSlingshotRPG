using UnityEngine;

public class UltimateSkill : BaseSkill
{
    private ICharacter owner;
    private float cooldownTime; // 쿨타임
    private float lastUsedTime; // 마지막 사용 시간

    public UltimateSkill(ICharacter owner, string name, string description, float cooldown) : base(name, description)
    {
        this.owner = owner;
        cooldownTime = cooldown;
        lastUsedTime = -cooldown; // 초기에는 바로 사용 가능하도록
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
            // 여기에 궁극기 스킬의 실제 로직 구현 (예: 광역 데미지, 버프)
            lastUsedTime = Time.time;
        }
        else
        {
            Debug.Log($"{SkillName} is on cooldown. Remaining: {cooldownTime - (Time.time - lastUsedTime):F2}s");
        }
    }
}