using UnityEngine;

public abstract class BaseSkill : ISkill
{
    public string SkillName { get; protected set; }
    public string Description { get; protected set; }

    public BaseSkill(string name, string description)
    {
        SkillName = name;
        Description = description;
    }

    public abstract void Execute();
}