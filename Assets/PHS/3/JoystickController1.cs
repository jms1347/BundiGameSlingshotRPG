using UnityEngine;
using UnityEngine.UI;

public class JoystickController1 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject joystickUI;
    public RectTransform handle;
    public Slider slingShotSlider;         // 드래그(슬링샷) 슬라이더
    public Slider chargingSlider;          // 차징(특수게이지) 슬라이더

    [Header("Force & Drag Settings")]
    public float maxDragDistance = 300f;    // 최대 드래그 거리
    public float maxForce = 10f;            // 기본 발사(이동) 힘

    [Header("Extra Charge Settings")]
    [SerializeField] private float maxExtraChargeDuration = 2f;  // 차징 게이지가 채워지는 시간 (0~2초)
    [SerializeField] private float extraMultiplierMax = 0.5f;      // 최대 추가 배수

    [Header("Special Attack Transition")]
    public float specialAttackThreshold = 3f;  // 차징 시간이 3초 이상이면 특수 공격 전환

    [Header("Special Gauge Settings")]
    public float specialCountdownDuration = 3f;  // 특수 공격 후 게이지 카운트다운 (미사용)

    [Header("FOV Settings")]
    // FOV 관련 처리는 CameraController에서 관리합니다.

    [Header("Debug Info")]
    public float attackMultiplier { get; private set; } = 1f;   // 최종 공격 배수

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
    public float touchCooldown = 0.2f;

    // 상태 변수
    private bool isFlying = false;
    private Vector3 lockedDirection;

    // 추가 공격력 누적 변수
    private float extraChargeTime = 0f;  // 누적 시간 (최대 3초까지)
    private int lastLoggedChargeCount = 0;

    // 특수 공격 모듈 (없으면 기본 특수 공격 실행)
    public SpecialAttackBehavior specialAttack;

    // 특수 게이지 관련 변수 (감소 기능은 제거, 단지 최대 상태 유지)
    private bool isSpecialGaugeActive = false;

    // "게이지가 최대입니다!" 로그를 한 번만 출력하는 플래그
    private bool hasLoggedMaxGauge = false;

    void Start()
    {
        joystickUI.SetActive(false);
        characterRigidbody = character.GetComponent<Rigidbody>();
        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                           RigidbodyConstraints.FreezeRotationZ |
                                           RigidbodyConstraints.FreezeRotationY |
                                           RigidbodyConstraints.FreezePositionY;
        cameraController = Camera.main.GetComponent<CameraController>();

        // 차징 슬라이더는 처음에 숨김
        if (chargingSlider != null)
            chargingSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleJoystickInput();
        HandleFlyingState();
        UpdateSpecialGauge();
    }

    private void HandleJoystickInput()
    {
        Vector2 touchPosition = Vector2.zero;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (touch.phase == TouchPhase.Began && !joystickUI.activeSelf)
                isTouching = false;
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
                        OnTouchMove(touchPosition);
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
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!joystickUI.activeSelf)
                    isTouching = false;
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
            if (velocity.magnitude < 0.1f)
            {
                isFlying = false;
                characterRigidbody.linearVelocity = Vector3.zero;
                characterRigidbody.angularVelocity = Vector3.zero;
                extraChargeTime = 0f;
                attackMultiplier = 1f;
                lastLoggedChargeCount = 0;
                hasLoggedMaxGauge = false;
                isSpecialGaugeActive = false;
                if (chargingSlider != null)
                    chargingSlider.gameObject.SetActive(false);
                if (cameraController != null)
                    cameraController.ResetChargeRatio();
            }
            else
            {
                rotationDummy.forward = lockedDirection;
            }
        }
    }

    private void UpdateSpecialGauge()
    {
        if (isSpecialGaugeActive && chargingSlider != null)
            chargingSlider.value = 1f;
    }

    private void OnTouchStart(Vector2 position)
    {
        if (isFlying) return;
        joystickUI.transform.position = position;
        joystickUI.SetActive(true);
        slingShotSlider.gameObject.SetActive(true);
        slingShotSlider.value = 0;
        startTouchPosition = position;
        handle.anchoredPosition = Vector2.zero;
        characterAnimator.SetBool("IsCharging", true);
        extraChargeTime = 0f;
        attackMultiplier = 1f;
        lastLoggedChargeCount = 0;
        hasLoggedMaxGauge = false;
        isSpecialGaugeActive = false;
        if (chargingSlider != null)
        {
            // 여기서 차징 게이지바는 슬링샷 게이지가 1이 되어야 나타나도록 함
            chargingSlider.gameObject.SetActive(false);
            chargingSlider.value = 0;
        }
    }

    private void OnTouchMove(Vector2 position)
    {
        if (isFlying) return;
        Vector2 currentDragPosition = position - startTouchPosition;
        float dragDistance = Mathf.Clamp(currentDragPosition.magnitude, 0, maxDragDistance);
        handle.anchoredPosition = currentDragPosition.normalized * dragDistance;
        slingShotSlider.value = dragDistance / maxDragDistance;
        Vector3 lookDirection = new Vector3(-currentDragPosition.x, 0, -currentDragPosition.y);
        if (lookDirection.magnitude > 0.1f)
            rotationDummy.forward = lookDirection.normalized;
        if (slingShotSlider.value < 1f)
        {
            extraChargeTime = 0f;
            attackMultiplier = 1f;
            lastLoggedChargeCount = 0;
            hasLoggedMaxGauge = false;
            if (chargingSlider != null)
                chargingSlider.value = 0;
            if (cameraController != null)
            {
                cameraController.SetSlingshotRatio(slingShotSlider.value);
                cameraController.SetAdditionalChargeRatio(0f);
            }
            // 차징 게이지바는 슬링샷 게이지가 최대치가 아니라면 숨김
            if (chargingSlider != null)
                chargingSlider.gameObject.SetActive(false);
        }
        else
        {
            // 슬링샷 게이지가 최대치라면 차징 게이지바를 표시
            if (chargingSlider != null && !chargingSlider.gameObject.activeSelf)
                chargingSlider.gameObject.SetActive(true);
            if (cameraController != null)
                cameraController.SetSlingshotRatio(1f);
            extraChargeTime += Time.deltaTime;
            if (extraChargeTime > maxExtraChargeDuration)
                extraChargeTime = maxExtraChargeDuration; // 여기서 게이지바는 0~2초까지 채워짐 (최대)
            attackMultiplier = Mathf.Clamp(
                Mathf.Lerp(1f, 1f + extraMultiplierMax, extraChargeTime / maxExtraChargeDuration),
                1f, 1f + extraMultiplierMax);
            if (chargingSlider != null)
            {
                float sliderValue = (extraChargeTime < maxExtraChargeDuration) ? (extraChargeTime / maxExtraChargeDuration) : 1f;
                chargingSlider.value = sliderValue;
                // 요청: 차징 게이지바가 1에 도달하면 바로 "게이지가 최대입니다!" 로그 출력
                if (sliderValue >= 1f && !hasLoggedMaxGauge)
                {
                    Debug.Log("게이지가 최대입니다!");
                    hasLoggedMaxGauge = true;
                }
                if (cameraController != null)
                    cameraController.SetAdditionalChargeRatio(chargingSlider.value);
            }
            int currentChargeCount = Mathf.FloorToInt(extraChargeTime);
            if (currentChargeCount != lastLoggedChargeCount)
            {
                Debug.Log("Extra Charge Time: " + currentChargeCount + " sec, Attack Multiplier: " + Mathf.FloorToInt(attackMultiplier));
                lastLoggedChargeCount = currentChargeCount;
            }
        }
    }

    private void OnTouchEnd()
    {
        if (isFlying) return;
        joystickUI.SetActive(false);
        slingShotSlider.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        characterAnimator.SetBool("IsCharging", false);
        characterAnimator.SetTrigger("Attack");
        float force = slingShotSlider.value * maxForce * attackMultiplier;
        lockedDirection = rotationDummy.forward;
        // 특수 공격 전환 조건: chargingSlider의 값이 1이면 특수 공격 실행
        if (chargingSlider != null && chargingSlider.value >= 1f)
        {
            if (specialAttack != null)
            {
                specialAttack.ExecuteSpecialAttack(character.position, lockedDirection, force);
            }
            else
            {
                Debug.Log("No special attack assigned. Executing default A-type special attack.");
                DefaultSpecialAttack(character.position, lockedDirection, force);
            }
            if (chargingSlider != null)
            {
                isSpecialGaugeActive = true;
                chargingSlider.value = 1f;
            }
        }
        else
        {
            characterRigidbody.AddForce(lockedDirection * force, ForceMode.Impulse);
            Debug.Log("Normal Fling Attack executed. Force: " + force);
        }
        isFlying = true;
        if (cameraController != null)
        {
            cameraController.SetCharging(false);
            cameraController.ResetChargeRatio();
        }
        Debug.Log("Final Attack Multiplier: " + Mathf.FloorToInt(attackMultiplier) + " / Launch Force: " + force);
    }

    private void DefaultSpecialAttack(Vector3 origin, Vector3 direction, float force)
    {
        characterRigidbody.AddForce(direction * force, ForceMode.Impulse);
        Debug.Log("Default Special Attack executed. Force: " + force);
    }
}