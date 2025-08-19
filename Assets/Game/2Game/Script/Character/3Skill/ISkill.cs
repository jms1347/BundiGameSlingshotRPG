public interface ISkill
{
    string SkillName { get; }
    string Description { get; }
    void Execute(); // 스킬 발동
}