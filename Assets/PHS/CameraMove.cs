using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    public Transform target; // 따라갈 캐릭터
    public float smoothSpeed = 0.125f; // 부드러운 이동 속도
    public Vector3 offset; // 고정된 오프셋 값 (높이와 각도 포함)

    private Vector3 fixedRotation; // 고정된 회전 각도

    void Start()
    {
        // 초기 회전값 저장
        fixedRotation = transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 목표 위치 계산
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 회전 값 고정
            transform.eulerAngles = fixedRotation;
        }
    }
}