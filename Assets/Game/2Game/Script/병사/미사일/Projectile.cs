using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    public float projectileSpeed = 10f; // ����ü �̵� �ӵ�
    public float pierceCount = 1;       // ���� ���
    public float lifetime = 5f;         // ����ü ���� �ֱ� (���� �ð� �� �ڵ� �Ҹ�)

    private Vector2 direction;
    private int damage;
    private GameObject owner;
    private int currentPiercedCount = 0; // ���� ������ ���� ��

    public void Initialize(Vector2 dir, int dmg, GameObject ownerObj)
    {
        direction = dir;
        damage = dmg;
        owner = ownerObj;

        // Ư�� �ð� �� �ڵ� �Ҹ�
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // ����ü �̵�
        transform.position += (Vector3)direction * projectileSpeed * Time.deltaTime;

        // �Ÿ��� ���� ������ �� ������ ���� ����
        float distance = Vector2.Distance(owner.transform.position, transform.position);

        // ����ü ũ�� ���� (�����̼� ��� Ŀ����, �־����� �۾���)
        // Lerp �Լ��� ����Ͽ� �ε巯�� ������ ��ȭ ����
        float scaleFactor = 1f;
        if (distance > owner.GetComponent<Archer>().minDamageScaleDistance)
        {
            float t = Mathf.InverseLerp(owner.GetComponent<Archer>().minDamageScaleDistance, owner.GetComponent<Archer>().maxDamageScaleDistance, distance);
            scaleFactor = Mathf.Lerp(1f, 0.5f, t); // 1.0f���� 0.5f���� ũ�� ����
        }
        transform.localScale = Vector3.one * scaleFactor;

        // ������ ��� (����ü ������ ������ŭ ������ ����)
        // damage = (int)(originalDamage * scaleFactor); // �� ����� Update���� �Ź� �������� �����ؾ� �ؼ� ȿ�������� ����
        // �浹 �ÿ� �������� ������� �������� ����ϴ� ���� �� ȿ�����Դϴ�.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� �� �±׸� �������� Ȯ��
        if (other.CompareTag(owner.GetComponent<SoldierBase>().enemyTag))
        {
            // ���� ��� Ȯ��
            if (currentPiercedCount < pierceCount)
            {
                // ������ ���
                int finalDamage = (int)(damage * transform.localScale.x);

                // ������ ������ ����
                SoldierBase enemy = other.GetComponent<SoldierBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(finalDamage);
                    currentPiercedCount++;
                    Debug.Log($"Projectile hit {other.name}. Pierced count: {currentPiercedCount}/{pierceCount}.");
                }
            }

            // ���� Ƚ���� ��� �����߰ų�, ���� ����� 0�̸� ����ü ��Ȱ��ȭ
            if (currentPiercedCount >= pierceCount)
            {
                gameObject.SetActive(false); // ��Ȱ��ȭ
                Destroy(gameObject, 0.5f); // ��Ȱ��ȭ �� ���� �ð� �� �ı� (�޸� ����)
            }
        }
    }
}