using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Stats")]
    public int attackPower = 10;             // �÷��̾��� ���ݷ�
    public float knockbackPower = 1.0f;        // �÷��̾��� �˹� ��ȭ ��ġ
    public float baseKnockbackForce = 10f;     // �⺻ �˹� ��

    // ������ ������ �������� �� ȣ��Ǵ� �޼��� (��: �ִϸ��̼� �̺�Ʈ�� �浹 �̺�Ʈ ��� ȣ��)
    public void ApplyAttack(GameObject enemy)
    {
        // ���� EnemyController ������Ʈ�� �����ɴϴ�.
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            // ���� �˹� ����(���°� ����� ����)�� 0 ������ ��� 1�� ó���Ͽ� 0���� ������ ������ �����մϴ�.
            float enemyResistance = enemyController.knockbackResistance;
            if (enemyResistance <= 0)
                enemyResistance = 1f;

            // �÷��̾��� ���ݷ°� knockbackPower�� ���� ����, �⺻ �˹� ���� ���ϰ�, ���� �������� ������ ���� �˹� ���� ����մϴ�.
            float effectiveKnockback = baseKnockbackForce * (knockbackPower * attackPower / enemyResistance);
            Debug.Log("Effective Knockback Force: " + effectiveKnockback);

            // ������ �˹��� �����մϴ�.
            enemyController.ApplyKnockback(effectiveKnockback, transform.position);
        }
        else
        {
            Debug.LogWarning("�� ��ü�� EnemyController ������Ʈ�� �����ϴ�.");
        }
    }
}