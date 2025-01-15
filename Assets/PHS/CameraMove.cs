using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    public Transform target; // ���� ĳ����
    public float smoothSpeed = 0.125f; // �ε巯�� �̵� �ӵ�
    public Vector3 offset; // ������ ������ �� (���̿� ���� ����)

    private Vector3 fixedRotation; // ������ ȸ�� ����

    void Start()
    {
        // �ʱ� ȸ���� ����
        fixedRotation = transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // ��ǥ ��ġ ���
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // ȸ�� �� ����
            transform.eulerAngles = fixedRotation;
        }
    }
}