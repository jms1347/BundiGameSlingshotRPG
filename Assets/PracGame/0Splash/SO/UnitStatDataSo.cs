using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int code;
    public string name;
    public float maxHp;
    public float moveSpeed;
    public float chaseRange;
    public float attackRange;
    public float attackPower;
    public float attackSpeed;
    public float defenseArmor;
}

[CreateAssetMenu(fileName = "UnitDataSo", menuName = "ScriptableObject/UnitDataSo")]
public class UnitStatDataSo : ScriptableObject
{
    public List<UnitData> unitDataList = new List<UnitData>();

    public UnitData GetUnitStatData(int pCode)
    {
        return unitDataList.Where(temp => temp.code.Equals(pCode)).FirstOrDefault();
    }
}
