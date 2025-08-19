using UnityEngine;
public enum MonsterState
{
    IDLE = 0,
    MOVE = 1,
    ATTACK = 2,
    HIT = 3,
    DEATH = 4
}

public enum UnitState
{
    IDLE = 0,
    MOVE = 1,
    ATTACK = 2,
    DAMAGEHIT = 3,
    DEATH = 4,
    // 필요에 따라 더 많은 상태 추가: SKILL, CASTING, STUNNED, ROOTED 등
}
public class GameEnums : MonoBehaviour
{

}
