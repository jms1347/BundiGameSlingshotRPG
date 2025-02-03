using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;                       // 플레이어를 따라갈 대상
    public Vector3 offset = new Vector3(0, 10, -10); // 카메라와 플레이어 간 오프셋
    public float followSpeed = 5f;                   // 플레이어 따라가는 속도

    public float defaultFOV = 60f;                   // 기본 FOV
    public float chargingFOV = 80f;                  // 차징 시 FOV
    public float fovTransitionSpeed = 5f;            // FOV 전환 속도

    private Camera mainCamera;
    public Camera uiCamera;                        // Inspector에서 할당할 UI 카메라

    private bool isCharging = false;

    void Start()
    {
        // 메인 카메라를 태그를 통해 찾음
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Please ensure the main camera is tagged as 'MainCamera'.");
        }

        // UI 카메라 초기화: 메인 카메라와 같은 FOV로 설정
        if (uiCamera != null)
        {
            uiCamera.fieldOfView = mainCamera.fieldOfView;
        }
    }

    void LateUpdate()
    {
        FollowPlayer();
        AdjustFOV();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void AdjustFOV()
    {
        if (mainCamera != null)
        {
            // 차징 여부에 따라 목표 FOV 결정
            float targetFOV = isCharging ? chargingFOV : defaultFOV;
            // 메인 카메라의 FOV를 부드럽게 변경
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);

            // UI 카메라가 할당되어 있다면 메인 카메라와 동일한 FOV 값으로 동기화
            if (uiCamera != null)
            {
                uiCamera.fieldOfView = mainCamera.fieldOfView;
            }
        }
    }

    // 외부에서 차징 상태를 변경할 수 있도록 하는 메서드
    public void SetCharging(bool charging)
    {
        isCharging = charging;
    }
}