using UnityEngine;

public class Swordsman : SoldierBase
{
    [Header("Swordsman Stats")]
    public float defenseMultiplier = 1.2f; // 방어력 계수 (기본 체력에 곱해짐)

    protected override void Awake()
    {
        // SoldierBase의 Awake 함수를 먼저 호출
        base.Awake();

        // 검병 고유의 능력치 설정
        moveSpeed = 2.5f;          // 검병은 방어에 강하므로 이동 속도는 다소 느리게
        attackSpeed = 1.0f;        // 공격 속도는 빠르게
        attackDamage = 15;         // 공격력은 준수하게
        currentHealth *= defenseMultiplier; // 방어력 계수를 적용하여 체력 증가
    }
}