using UnityEngine;

public class Missile : MonoBehaviour
{
    // �̻����� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 10f;
    // �̻����� �ִ� �����Ÿ�
    [SerializeField] private float maxDistance = 10f;

    private Transform target;
    private float damage;
    private Vector3 initialPosition;

    // �̻����� �߻��� ������ �±� (�ڽſ��� ���ظ� ���� �ʱ� ����)
    private string myUnitTag;
    // �̻��� �߻� �غ� �Լ�
    public void Launch(Transform _target, float _damage, Vector3 startPosition, string _myUnitTag)
    {
        // �̻����� �ʱ� ��ġ�� �߻� ��ġ�� ����
        transform.position = startPosition;
        // �̻����� ��ǥ�� ���ݷ� ����
        target = _target;
        damage = _damage;
        // �߻� ���� ��ġ ���� (���� �Ÿ� ����)
        initialPosition = startPosition;
        // �̻����� �߻��� ������ �±� ����
        myUnitTag = _myUnitTag;

        // �̻����� Ȱ��ȭ
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Ÿ���� ��ȿ���� �ʰų�, �̻����� �ִ� �����Ÿ��� �Ѿ�� ��Ȱ��ȭ
        if (target == null || Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            gameObject.SetActive(false);
            return;
        }

        // Ÿ���� ���� �̻��� �̵�
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� �ڽ�(�̻���)�� �߻�� ������ �ƴϰ�,
        // UnitBase ������Ʈ�� ������ ������, �ٸ� �±׸� ������ �ִ��� Ȯ��
        UnitBase otherUnit = other.GetComponent<UnitBase>();
        if (otherUnit != null && !other.CompareTag(myUnitTag))
        {
            // Ÿ���� �ƴϴ��� UnitBase�� ���� ���̶�� ������ ����
            otherUnit.TakeDamage(damage);

            // �������� �� �� �̻��� ��Ȱ��ȭ (�� ���� ���ظ� �ֵ���)
            gameObject.SetActive(false);
        }
    }
}
