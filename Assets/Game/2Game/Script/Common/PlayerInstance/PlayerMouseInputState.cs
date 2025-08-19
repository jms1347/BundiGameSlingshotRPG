using System;
using UnityEngine;

public class PlayerMouseInputState : MonoBehaviour
{
    public bool IsDownPressed { get; set; }
    public bool ISPressed { get; set; } // 오타: IsPressed로 수정하는 것이 좋습니다.
    public bool IsUpPressed { get; set; }

    public Vector3 LastGroundClickPosition { get; private set; }
    public RaycastHit LastRaycastHitInfo { get; private set; }
    public bool HasClickedGround { get; private set; }

    // 새로 추가: 현재 마우스 포인터의 최종 월드 좌표
    public Vector3 CurrentMouseWorldPosition { get; private set; } // 이전 답변에서 추가된 부분

    [SerializeField]
    private LayerMask groundLayer = LayerMask.GetMask("Ground"); // Ground 레이어 설정

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다. 씬에 Main Camera 태그가 지정된 카메라가 있는지 확인하세요.");
        }
    }

    // Update 대신 GetCurrentMousePosition()이 외부에서 호출된다고 가정합니다.
    // 만약 Update에서 마우스 입력을 직접 처리한다면, Update 안에 GetCurrentMousePosition()을 호출해야 합니다.
    // 여기서는 GetCurrentMousePosition()이 마우스 클릭 상태에 따라 호출되도록 유지하겠습니다.
    public void CheckCurrentMousePosition()
    {
        if (ISPressed || IsUpPressed) // 셋 중 하나라도 true면 레이캐스트 수행
        {
            PerformRaycastBasedOnMode();
        }
    }

    public Vector3 GetCurrentMousePosition()
    {
        if (IsDownPressed ) // 셋 중 하나라도 true면 레이캐스트 수행
        {
            return GetRaycastPosition();
        }

        return this.transform.position;
    }

    private Vector3 GetRaycastPosition()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // 혹시라도 null이면 다시 찾기

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // --- 레이캐스트 로직 (이전 답변에서 개선된 우선순위 로직) ---

        // 1. Ground 레이어 우선 검사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            LastGroundClickPosition = hit.point;
            LastRaycastHitInfo = hit;
            HasClickedGround = true;
            CurrentMouseWorldPosition = hit.point;
            Debug.Log($"Ground 클릭! 좌표: {LastGroundClickPosition}, 오브젝트: {hit.collider.name}");

            return LastGroundClickPosition;
        }
        else
        {
            HasClickedGround = false;
            LastGroundClickPosition = Vector3.zero;
            LastRaycastHitInfo = new RaycastHit();

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0인 평면
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                CurrentMouseWorldPosition = ray.GetPoint(distance);

                return CurrentMouseWorldPosition;

            }
            else
            {
                CurrentMouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
                Debug.LogWarning("가상 평면과 교차하지 못했습니다. 카메라에서 임의의 거리를 사용합니다.");

                return CurrentMouseWorldPosition;

            }
            Debug.Log($"아무것도 클릭하지 않음. 마우스 포인터 월드 좌표: {CurrentMouseWorldPosition}");
        }
    }


    private void PerformRaycastBasedOnMode()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // 혹시라도 null이면 다시 찾기

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // --- 레이캐스트 로직 (이전 답변에서 개선된 우선순위 로직) ---

        // 1. Ground 레이어 우선 검사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            LastGroundClickPosition = hit.point;
            LastRaycastHitInfo = hit;
            HasClickedGround = true;
            CurrentMouseWorldPosition = hit.point;
            Debug.Log($"Ground 클릭! 좌표: {LastGroundClickPosition}, 오브젝트: {hit.collider.name}");
        }
        else
        {
            HasClickedGround = false;
            LastGroundClickPosition = Vector3.zero;
            LastRaycastHitInfo = new RaycastHit();

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0인 평면
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                CurrentMouseWorldPosition = ray.GetPoint(distance);
            }
            else
            {
                CurrentMouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
                Debug.LogWarning("가상 평면과 교차하지 못했습니다. 카메라에서 임의의 거리를 사용합니다.");
            }

            Debug.Log($"아무것도 클릭하지 않음. 마우스 포인터 월드 좌표: {CurrentMouseWorldPosition}");
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