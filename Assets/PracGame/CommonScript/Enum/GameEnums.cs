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
    // �ʿ信 ���� �� ���� ���� �߰�: SKILL, CASTING, STUNNED, ROOTED ��
}
public class GameEnums : MonoBehaviour
{

}
