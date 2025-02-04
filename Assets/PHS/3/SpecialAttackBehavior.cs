using UnityEngine;

// Ư�� ���� ��� �������̽�
public interface IAttackBehavior
{
    /// <summary>
    /// Ư�� ���� ����.
    /// </summary>
    /// <param name="origin">���� ���� ��ġ</param>
    /// <param name="direction">���� ����</param>
    /// <param name="force">���ݿ� ������ ��</param>
    void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force);
}

// MonoBehaviour�� ����Ͽ� Inspector���� �Ҵ� �����ϵ��� �߻� Ŭ���� ����
public abstract class SpecialAttackBehavior : MonoBehaviour, IAttackBehavior
{
    public abstract void ExecuteSpecialAttack(Vector3 origin, Vector3 direction, float force);
}