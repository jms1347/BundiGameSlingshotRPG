using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatData
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 3f;
    public float chaseRange = 15f;
    public float attackRange = 1f;
    public float AttackCooldown = 1f;
    public float AttackDamage = 10f;

}
[CreateAssetMenu(fileName = "CharacterStatDataSO", menuName = "ScriptableObject/CharacterStatDataSO")]
public class CharacterStatDataSo : ScriptableObject
{
    public List<CharacterStatData> characterStatDataList = new List<CharacterStatData>();

}
