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
    public int iconIndex; // UI���� ����� ������ �ε��� �Ǵ� Sprite ����

    public float cooldown;
    public float damage; // �⺻ ������ ��ġ
    public float mpCost; // ��ų ��뿡 �ʿ��� ���� �� ���ҽ�
    public float castingTime;
    public SkillType skillType; // ��: Active, Passive, Ultimate ��

   
}
[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObject/SkillDataSO")]
public class SkillDataSo : ScriptableObject
{
    public List<SkillData> skillDataList = new List<SkillData>();
}
