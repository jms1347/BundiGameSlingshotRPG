using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 플레이어를 따라감
    public Vector3 offset = new Vector3(0, 10, -10); // 카메라와 플레이어의 거리
    public float followSpeed = 5f; // 따라가는 속도

    public float defaultFOV = 60f; // 기본 FOV
    public float chargingFOV = 80f; // 차징 시 FOV
    public float fovTransitionSpeed = 5f; // FOV 전환 속도

    private Camera mainCamera;
    private bool isCharging = false;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Please ensure the main camera is tagged as 'MainCamera'.");
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
            float targetFOV = isCharging ? chargingFOV : defaultFOV;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        }
    }

    public void SetCharging(bool charging)
    {
        isCharging = charging;
    }
}