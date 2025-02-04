using UnityEngine;

public class ProjectileAttackBehavior : SpecialAttackBehavior
{
    public GameObject projectilePrefab;    // ����ü ������
    public Transform projectileSpawnPoint; // ����ü ���� ��ġ

    public override void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force)
    {
        if (projectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("ProjectileAttackBehavior: Missing prefab or spawn point!");
            return;
        }
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
        Debug.Log("Projectile Attack executed. Force: " + force);
    }
}