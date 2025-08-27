using UnityEngine;

public class Archer : SoldierBase
{
    [Header("Archer Stats")]
    public GameObject projectilePrefab; // 발사할 투사체 프리팹
    public float minDamageScaleDistance = 5f; // 데미지 감소가 시작되는 거리
    public float maxDamageScaleDistance = 10f; // 데미지 감소가 최대치에 도달하는 거리

    protected override void Awake()
    {
        base.Awake();

        // 궁병 고유의 능력치 설정
        moveSpeed = 3.0f;          // 적당한 이동 속도
        attackSpeed = 2.0f;        // 공격 속도는 검병보다 느리게
        attackDamage = 20;         // 기본 공격력은 높게
        searchRadius = 15f;        // 원거리 유닛이므로 탐지 반경을 넓게
    }

    protected override void AttackTarget()
    {
        if (targetEnemy == null || projectilePrefab == null) return;

        // 투사체 생성
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            // 투사체에 필요한 정보 전달
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            projectile.Initialize(direction, attackDamage, this.gameObject);
        }

        // 마지막 공격 시간 갱신
        lastAttackTime = Time.time;
    }
}