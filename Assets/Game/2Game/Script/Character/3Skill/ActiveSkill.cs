using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 이 스킬을 사용하는 캐릭터 참조
    private ICharacter owner;

    public ActiveSkill(ICharacter owner, string name, string description) : base(name, description)
    {
        this.owner = owner;
    }

    public override void Execute()
    {
        Debug.Log($"{owner.Name} uses Active Skill: {SkillName}");
        // 여기에 액티브 스킬의 실제 로직 구현 (예: 특정 적에게 데미지)
        // 예를 들어, 주변 적에게 10 데미지
        // CombatManager.Instance.DealDamageToEnemies(10);
    }
}