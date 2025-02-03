using UnityEngine;
using UnityEngine.UI;

public class JoystickController1 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject joystickUI;
    public RectTransform handle;
    public Slider chargeSlider;

    [Header("Force & Drag Settings")]
    public float maxDragDistance = 300f;    // 최대 드래그 거리 (이 값 이상이면 충전바는 1)
    public float maxForce = 10f;            // 기본 발사(이동) 힘 (chargeSlider.value와 곱해져서 사용)
    public float knockbackForce = 10f;      // 적에게 적용할 기본 넉백 힘 (별도로 사용)
    public float playerKnockbackForce = 15f; // 플레이어가 충돌 시 밀려나는 힘 (필요에 따라 사용)

    [Header("Extra Charge Settings")]
    [SerializeField] private float maxExtraChargeDuration = 3f;  // 최대 추가 누적 시간 (초)
    [SerializeField] private float extraMultiplierMax = 0.5f;      // 최대 추가 배수 (기본 1에서 최대 1+extraMultiplierMax까지)

    [Header("Debug Info")]
    public float attackMultiplier { get; private set; } = 1f;    // 최종 공격 배수 (Inspector에서 확인 가능)

    [Header("References")]
    public Transform rotationDummy; // 회전 전용 더미
    public Transform character;     // 이동 담당
    public Animator characterAnimator;

    private Rigidbody characterRigidbody;
    private CameraController cameraController;

    // 입력 관련 변수
    private Vector2 startTouchPosition;
    private bool isTouching = false;
    private float lastTouchTime = 0f;
    public float touchCooldown = 0.2f; // 최소 터치 간격 (초)

    // 상태 변수
    private bool isFlying = false;
    private Vector3 lockedDirection; // 비행 방향 고정용

    // 추가 공격력 누적 변수 (충전바가 최대치일 때부터 누적)
    private float extraChargeTime = 0f;

    void Start()
    {
        joystickUI.SetActive(false);
        characterRigidbody = character.GetComponent<Rigidbody>();

        // 캐릭터의 Y축 이동 및 회전 고정 (필요한 축만 고정)
        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                           RigidbodyConstraints.FreezeRotationZ |
                                           RigidbodyConstraints.FreezeRotationY |
                                           RigidbodyConstraints.FreezePositionY;

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

        // 모바일 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            // 새 터치가 시작되었는데 조이스틱 UI가 비활성 상태라면 isTouching 플래그 초기화
            if (touch.phase == TouchPhase.Began && !joystickUI.activeSelf)
            {
                isTouching = false;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isTouching && (Time.time - lastTouchTime >= touchCooldown))
                    {
                        isTouching = true;
                        lastTouchTime = Time.time;
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
                        lastTouchTime = Time.time;
                    }
                    break;
            }
        }

        // 에디터/PC 환경에서의 마우스 입력 처리
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 새 터치인데 조이스틱 UI가 비활성 상태면 isTouching 플래그 초기화
                if (!joystickUI.activeSelf)
                {
                    isTouching = false;
                }

                if (!isTouching && (Time.time - lastTouchTime >= touchCooldown))
                {
                    isTouching = true;
                    lastTouchTime = Time.time;
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
                    lastTouchTime = Time.time;
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
                characterRigidbody.angularVelocity = Vector3.zero;
                // 비행 종료 시 추가 공격력 관련 변수 초기화
                extraChargeTime = 0f;
                attackMultiplier = 1f;
            }
            else
            {
                // 비행 중에는 고정된 방향 유지
                rotationDummy.forward = lockedDirection;
            }
        }
    }

    private void OnTouchStart(Vector2 position)
    {
        if (isFlying) return; // 비행 중이면 터치 무시

        // 터치 시작 시 조이스틱 UI 활성화 및 초기화
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

        // 추가 공격력 관련 변수 초기화
        extraChargeTime = 0f;
        attackMultiplier = 1f;
    }

    private void OnTouchMove(Vector2 position)
    {
        if (isFlying) return;

        Vector2 currentDragPosition = position - startTouchPosition;
        // 드래그 거리를 최대 maxDragDistance로 제한
        float dragDistance = Mathf.Clamp(currentDragPosition.magnitude, 0, maxDragDistance);
        handle.anchoredPosition = currentDragPosition.normalized * dragDistance;
        // chargeSlider.value는 0 ~ 1 (최대: maxDragDistance)
        chargeSlider.value = dragDistance / maxDragDistance;

        // 회전 더미 업데이트 (드래그 방향에 따라)
        Vector3 lookDirection = new Vector3(-currentDragPosition.x, 0, -currentDragPosition.y);
        if (lookDirection.magnitude > 0.1f)
        {
            rotationDummy.forward = lookDirection.normalized;
        }

        // 충전바가 최대치에 도달했다면, 추가 누적 시간(extraChargeTime)과 attackMultiplier 업데이트
        if (chargeSlider.value >= 1f)
        {
            extraChargeTime += Time.deltaTime;
            // extraChargeTime에 따라 attackMultiplier를 1에서 (1 + extraMultiplierMax)까지 선형 보간
            attackMultiplier = Mathf.Clamp(Mathf.Lerp(1f, 1f + extraMultiplierMax, extraChargeTime / maxExtraChargeDuration), 1f, 1f + extraMultiplierMax);
            // 디버그 로그로 누적 시간과 attackMultiplier 출력
            Debug.Log("Extra Charge Time: " + extraChargeTime.ToString("F2") + " sec, Attack Multiplier: " + attackMultiplier.ToString("F2"));
        }
        else
        {
            // 충전바가 최대치 미만이면 추가 누적 시간 및 공격 배수 초기화
            extraChargeTime = 0f;
            attackMultiplier = 1f;
        }
    }

    private void OnTouchEnd()
    {
        if (isFlying) return;

        // 터치 종료 시 조이스틱 UI 숨김
        joystickUI.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        characterAnimator.SetBool("IsCharging", false);
        characterAnimator.SetTrigger("Attack");

        // 이동(발사) 힘 계산: chargeSlider.value에 maxForce와 attackMultiplier를 곱해 최종 힘 산출
        float force = chargeSlider.value * maxForce * attackMultiplier;
        lockedDirection = rotationDummy.forward;
        Vector3 launchDirection = lockedDirection * force;
        characterRigidbody.AddForce(launchDirection, ForceMode.Impulse);
        isFlying = true;

        // 카메라에 차징 상태 해제 알림
        if (cameraController != null)
        {
            cameraController.SetCharging(false);
        }

        Debug.Log("Final Attack Multiplier: " + attackMultiplier + " / Launch Force: " + force);
    }
}