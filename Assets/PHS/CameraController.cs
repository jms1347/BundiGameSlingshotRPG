using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;

    [Header("FOV Settings")]
    public float defaultFOV = 60f;    // 기본 시야각
    public float midFOV = 65f;        // 슬링샷 게이지(일반)가 최대일 때 도달하는 시야각
    public float chargingFOV = 80f;   // 차징 게이지(추가)가 최대일 때 도달하는 시야각
    public float fovLerpSpeed = 5f;   // FOV 보간 속도

    [Header("UI Camera")]
    public Camera uiCamera;         // UI에 사용하는 카메라 (Perspective 모드여야 함)

    private Camera mainCamera;
    private float slingshotRatio = 0f;         // 0~1, 슬링샷 게이지 비율
    private float additionalChargeRatio = 0f;   // 0~1, 추가 차징 게이지 비율

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
            mainCamera.fieldOfView = defaultFOV;
        if (uiCamera != null)
            uiCamera.fieldOfView = defaultFOV;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPos = player.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }

        // 먼저 슬링샷 게이지에 따라 기본 FOV에서 중간 FOV로 보간
        float targetFOV = Mathf.Lerp(defaultFOV, midFOV, slingshotRatio);
        // 그 후, 추가 차징 게이지에 따라 중간 FOV에서 chargingFOV까지 보간
        // 단, 추가 차징 비율은 최대 0.75까지 반영 (예: 게이지 최대치의 2/3까지)
        float effectiveAdditional = Mathf.Clamp(additionalChargeRatio, 0f, 0.75f);
        targetFOV = Mathf.Lerp(targetFOV, chargingFOV, effectiveAdditional);

        if (mainCamera != null)
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
        if (uiCamera != null)
            uiCamera.fieldOfView = mainCamera.fieldOfView;
    }

    // 슬링샷 게이지 비율 설정 (0~1)
    public void SetSlingshotRatio(float ratio)
    {
        slingshotRatio = Mathf.Clamp01(ratio);
    }

    // 추가 차징 게이지 비율 설정 (0~1)
    public void SetAdditionalChargeRatio(float ratio)
    {
        additionalChargeRatio = Mathf.Clamp01(ratio);
    }

    public void ResetChargeRatio()
    {
        slingshotRatio = 0f;
        additionalChargeRatio = 0f;
    }

    public void SetCharging(bool charging)
    {
        if (!charging)
            ResetChargeRatio();
    }
}