using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;                       // �÷��̾ ���� ���
    public Vector3 offset = new Vector3(0, 10, -10); // ī�޶�� �÷��̾� �� ������
    public float followSpeed = 5f;                   // �÷��̾� ���󰡴� �ӵ�

    public float defaultFOV = 60f;                   // �⺻ FOV
    public float chargingFOV = 80f;                  // ��¡ �� FOV
    public float fovTransitionSpeed = 5f;            // FOV ��ȯ �ӵ�

    private Camera mainCamera;
    public Camera uiCamera;                        // Inspector���� �Ҵ��� UI ī�޶�

    private bool isCharging = false;

    void Start()
    {
        // ���� ī�޶� �±׸� ���� ã��
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Please ensure the main camera is tagged as 'MainCamera'.");
        }

        // UI ī�޶� �ʱ�ȭ: ���� ī�޶�� ���� FOV�� ����
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
            // ��¡ ���ο� ���� ��ǥ FOV ����
            float targetFOV = isCharging ? chargingFOV : defaultFOV;
            // ���� ī�޶��� FOV�� �ε巴�� ����
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);

            // UI ī�޶� �Ҵ�Ǿ� �ִٸ� ���� ī�޶�� ������ FOV ������ ����ȭ
            if (uiCamera != null)
            {
                uiCamera.fieldOfView = mainCamera.fieldOfView;
            }
        }
    }

    // �ܺο��� ��¡ ���¸� ������ �� �ֵ��� �ϴ� �޼���
    public void SetCharging(bool charging)
    {
        isCharging = charging;
    }
}