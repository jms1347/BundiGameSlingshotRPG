using System;
using UnityEngine;

public class PlayerMouseInputState : MonoBehaviour
{
    public bool IsDownPressed { get; set; }
    public bool ISPressed { get; set; } // ��Ÿ: IsPressed�� �����ϴ� ���� �����ϴ�.
    public bool IsUpPressed { get; set; }

    public Vector3 LastGroundClickPosition { get; private set; }
    public RaycastHit LastRaycastHitInfo { get; private set; }
    public bool HasClickedGround { get; private set; }

    // ���� �߰�: ���� ���콺 �������� ���� ���� ��ǥ
    public Vector3 CurrentMouseWorldPosition { get; private set; } // ���� �亯���� �߰��� �κ�

    [SerializeField]
    private LayerMask groundLayer = LayerMask.GetMask("Ground"); // Ground ���̾� ����

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� ã�� �� �����ϴ�. ���� Main Camera �±װ� ������ ī�޶� �ִ��� Ȯ���ϼ���.");
        }
    }

    // Update ��� GetCurrentMousePosition()�� �ܺο��� ȣ��ȴٰ� �����մϴ�.
    // ���� Update���� ���콺 �Է��� ���� ó���Ѵٸ�, Update �ȿ� GetCurrentMousePosition()�� ȣ���ؾ� �մϴ�.
    // ���⼭�� GetCurrentMousePosition()�� ���콺 Ŭ�� ���¿� ���� ȣ��ǵ��� �����ϰڽ��ϴ�.
    public void CheckCurrentMousePosition()
    {
        if (ISPressed || IsUpPressed) // �� �� �ϳ��� true�� ����ĳ��Ʈ ����
        {
            PerformRaycastBasedOnMode();
        }
    }

    public Vector3 GetCurrentMousePosition()
    {
        if (IsDownPressed ) // �� �� �ϳ��� true�� ����ĳ��Ʈ ����
        {
            return GetRaycastPosition();
        }

        return this.transform.position;
    }

    private Vector3 GetRaycastPosition()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Ȥ�ö� null�̸� �ٽ� ã��

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // --- ����ĳ��Ʈ ���� (���� �亯���� ������ �켱���� ����) ---

        // 1. Ground ���̾� �켱 �˻�
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            LastGroundClickPosition = hit.point;
            LastRaycastHitInfo = hit;
            HasClickedGround = true;
            CurrentMouseWorldPosition = hit.point;
            Debug.Log($"Ground Ŭ��! ��ǥ: {LastGroundClickPosition}, ������Ʈ: {hit.collider.name}");

            return LastGroundClickPosition;
        }
        else
        {
            HasClickedGround = false;
            LastGroundClickPosition = Vector3.zero;
            LastRaycastHitInfo = new RaycastHit();

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0�� ���
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                CurrentMouseWorldPosition = ray.GetPoint(distance);

                return CurrentMouseWorldPosition;

            }
            else
            {
                CurrentMouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
                Debug.LogWarning("���� ���� �������� ���߽��ϴ�. ī�޶󿡼� ������ �Ÿ��� ����մϴ�.");

                return CurrentMouseWorldPosition;

            }
            Debug.Log($"�ƹ��͵� Ŭ������ ����. ���콺 ������ ���� ��ǥ: {CurrentMouseWorldPosition}");
        }
    }


    private void PerformRaycastBasedOnMode()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Ȥ�ö� null�̸� �ٽ� ã��

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // --- ����ĳ��Ʈ ���� (���� �亯���� ������ �켱���� ����) ---

        // 1. Ground ���̾� �켱 �˻�
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            LastGroundClickPosition = hit.point;
            LastRaycastHitInfo = hit;
            HasClickedGround = true;
            CurrentMouseWorldPosition = hit.point;
            Debug.Log($"Ground Ŭ��! ��ǥ: {LastGroundClickPosition}, ������Ʈ: {hit.collider.name}");
        }
        else
        {
            HasClickedGround = false;
            LastGroundClickPosition = Vector3.zero;
            LastRaycastHitInfo = new RaycastHit();

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0�� ���
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                CurrentMouseWorldPosition = ray.GetPoint(distance);
            }
            else
            {
                CurrentMouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
                Debug.LogWarning("���� ���� �������� ���߽��ϴ�. ī�޶󿡼� ������ �Ÿ��� ����մϴ�.");
            }

            Debug.Log($"�ƹ��͵� Ŭ������ ����. ���콺 ������ ���� ��ǥ: {CurrentMouseWorldPosition}");
        }
    }

    public void ResetClickInfo()
    {
        HasClickedGround = false;
        LastGroundClickPosition = Vector3.zero;
        LastRaycastHitInfo = new RaycastHit();
        CurrentMouseWorldPosition = Vector3.zero;
    }
}