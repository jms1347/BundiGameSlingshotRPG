using UnityEngine;
using UnityEngine.UI;

public class JoystickController1 : MonoBehaviour
{
    public GameObject joystickUI;
    public RectTransform handle;
    public Slider chargeSlider;
    public float maxDragDistance = 300f;
    public float maxForce = 10f;
    public float knockbackForce = 10f; // 적이 밀려나는 힘
    public float playerKnockbackForce = 15f; // 플레이어가 밀려나는 힘
    private Vector2 startTouchPosition;

    public Transform rotationDummy; // 회전 전용 더미
    public Transform character; // 이동 담당
    public Animator characterAnimator;
    private Rigidbody characterRigidbody;

    private bool isFlying = false;
    private Vector3 lockedDirection; // 비행 방향 고정용

    private CameraController cameraController; // 카메라 조정

    private bool isTouching = false; // 중복 입력 방지

    void Start()
    {
        joystickUI.SetActive(false);
        characterRigidbody = character.GetComponent<Rigidbody>();

        // Y축 회전 고정
        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;

        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        HandleJoystickInput();
        HandleFlyingState();
    }

    private void HandleJoystickInput()
    {
        Vector2 touchPosition = Vector2.zero;

        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isTouching) // 중복 입력 방지
                    {
                        isTouching = true;
                        OnTouchStart(touchPosition);
                    }
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isTouching)
                    {
                        OnTouchMove(touchPosition);
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isTouching)
                    {
                        OnTouchEnd();
                        isTouching = false;
                    }
                    break;
            }
        }

        // 마우스 입력 처리 (에디터 및 PC 환경)
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isTouching) // 중복 입력 방지
                {
                    isTouching = true;
                    touchPosition = Input.mousePosition;
                    OnTouchStart(touchPosition);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (isTouching)
                {
                    touchPosition = Input.mousePosition;
                    OnTouchMove(touchPosition);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isTouching)
                {
                    OnTouchEnd();
                    isTouching = false;
                }
            }
        }
    }

    private void HandleFlyingState()
    {
        if (isFlying)
        {
            Vector3 velocity = characterRigidbody.linearVelocity;
            if (velocity.magnitude < 0.1f) // 비행 종료 조건
            {
                isFlying = false;
                characterRigidbody.linearVelocity = Vector3.zero;
                characterRigidbody.angularVelocity = Vector3.zero; // 회전도 멈춤
            }
            else
            {
                // 비행 중 방향 고정
                rotationDummy.forward = lockedDirection;
            }
        }
    }

    private void OnTouchStart(Vector2 position)
    {
        if (isFlying) return; // 비행 중에는 조작 불가

        joystickUI.transform.position = position;
        joystickUI.SetActive(true);
        chargeSlider.gameObject.SetActive(true);
        chargeSlider.value = 0;
        startTouchPosition = position;

        handle.anchoredPosition = Vector2.zero;

        characterAnimator.SetBool("IsCharging", true);

        // 카메라에 차징 상태 알림
        if (cameraController != null)
        {
            cameraController.SetCharging(true);
        }
    }

    private void OnTouchMove(Vector2 position)
    {
        if (isFlying) return; // 비행 중에는 조작 불가

        Vector2 currentDragPosition = position - startTouchPosition;
        float dragDistance = Mathf.Clamp(currentDragPosition.magnitude, 0, maxDragDistance);

        handle.anchoredPosition = currentDragPosition.normalized * dragDistance;
        chargeSlider.value = dragDistance / maxDragDistance;

        // 회전 더미를 드래그 방향으로 조정
        Vector3 lookDirection = new Vector3(-currentDragPosition.x, 0, -currentDragPosition.y);
        if (lookDirection.magnitude > 0.1f)
        {
            rotationDummy.forward = lookDirection.normalized;
        }
    }

    private void OnTouchEnd()
    {
        if (isFlying) return; // 비행 중에는 조작 불가

        joystickUI.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;

        characterAnimator.SetBool("IsCharging", false);
        characterAnimator.SetTrigger("Attack");

        float force = chargeSlider.value * maxForce;
        lockedDirection = rotationDummy.forward;
        Vector3 launchDirection = lockedDirection * force;

        characterRigidbody.AddForce(launchDirection, ForceMode.Impulse);
        isFlying = true;

        // 카메라에 차징 상태 해제 알림
        if (cameraController != null)
        {
            cameraController.SetCharging(false);
        }
    }
}
