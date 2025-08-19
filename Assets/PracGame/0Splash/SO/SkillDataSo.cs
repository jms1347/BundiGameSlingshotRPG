using UnityEngine;
using System.Collections.Generic;
public enum SkillType
{
    Active,
    Buff,
    Passive,
    Ultimate
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    public string skillCode;
    [TextArea] public string skillDesc;
    public int iconIndex; // UI에서 사용할 아이콘 인덱스 또는 Sprite 참조

    public float cooldown;
    public float damage; // 기본 데미지 수치
    public float mpCost; // 스킬 사용에 필요한 마나 등 리소스
    public float castingTime;
    public SkillType skillType; // 예: Active, Passive, Ultimate 등

   
}
[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObject/SkillDataSO")]
public class SkillDataSo : ScriptableObject
{
    public List<SkillData> skillDataList = new List<SkillData>();
}
