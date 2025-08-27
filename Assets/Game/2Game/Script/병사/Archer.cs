using UnityEngine;

public class Archer : SoldierBase
{
    [Header("Archer Stats")]
    public GameObject projectilePrefab; // �߻��� ����ü ������
    public float minDamageScaleDistance = 5f; // ������ ���Ұ� ���۵Ǵ� �Ÿ�
    public float maxDamageScaleDistance = 10f; // ������ ���Ұ� �ִ�ġ�� �����ϴ� �Ÿ�

    protected override void Awake()
    {
        base.Awake();

        // �ú� ������ �ɷ�ġ ����
        moveSpeed = 3.0f;          // ������ �̵� �ӵ�
        attackSpeed = 2.0f;        // ���� �ӵ��� �˺����� ������
        attackDamage = 20;         // �⺻ ���ݷ��� ����
        searchRadius = 15f;        // ���Ÿ� �����̹Ƿ� Ž�� �ݰ��� �а�
    }

    protected override void AttackTarget()
    {
        if (targetEnemy == null || projectilePrefab == null) return;

        // ����ü ����
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            // ����ü�� �ʿ��� ���� ����
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            projectile.Initialize(direction, attackDamage, this.gameObject);
        }

        // ������ ���� �ð� ����
        lastAttackTime = Time.time;
    }
}