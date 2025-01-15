using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    public GameObject joystickUI;
    public RectTransform handle;
    public Slider chargeSlider;
    public float maxDragDistance = 300f;
    public float maxForce = 10f;
    public float knockbackForce = 10f; // 적이 밀려나는 힘
    public float playerKnockbackForce = 15f; // 플레이어가 밀려나는 힘
    private Vector2 startTouchPosition;

    public Transform character;
    public Animator characterAnimator;
    private Rigidbody characterRigidbody;

    public Transform[] enemies;
    public float enemyAttackInterval = 5f;
    public float enemySpeed = 2f;
    public float attackRange = 1.5f; // 적의 공격 범위

    private float[] enemyAttackTimers;

    void Start()
    {
        joystickUI.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        characterRigidbody = character.GetComponent<Rigidbody>();

        characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        enemyAttackTimers = new float[enemies.Length];
        for (int i = 0; i < enemyAttackTimers.Length; i++)
        {
            enemyAttackTimers[i] = enemyAttackInterval;
        }

        // 적의 Rigidbody 회전 고정 설정
        foreach (Transform enemy in enemies)
        {
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    void Update()
    {
        HandleJoystickInput();
        HandleEnemies();
    }

    private void HandleJoystickInput()
    {
        bool isTouching = false;
        Vector2 touchPosition = Vector2.zero;

        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchStart(touchPosition);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    OnTouchMove(touchPosition);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchEnd();
                    break;
            }
            isTouching = true;
        }

        // 마우스 입력 처리 (에디터 및 PC 환경)
        if (!isTouching)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchPosition = Input.mousePosition;
                OnTouchStart(touchPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                touchPosition = Input.mousePosition;
                OnTouchMove(touchPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnd();
            }
        }
    }

    private void OnTouchStart(Vector2 position)
    {
        joystickUI.transform.position = position;
        joystickUI.SetActive(true);
        chargeSlider.gameObject.SetActive(true);
        chargeSlider.value = 0;
        startTouchPosition = position;

        handle.anchoredPosition = Vector2.zero;

        Vector3 characterScreenPosition = Camera.main.WorldToScreenPoint(character.position);
        chargeSlider.transform.position = characterScreenPosition + new Vector3(0, -50, 0);

        characterAnimator.SetBool("IsCharging", true);
    }

    private void OnTouchMove(Vector2 position)
    {
        Vector2 currentDragPosition = position - startTouchPosition;
        float dragDistance = Mathf.Clamp(currentDragPosition.magnitude, 0, maxDragDistance);

        handle.anchoredPosition = dragDistance <= maxDragDistance
            ? currentDragPosition
            : currentDragPosition.normalized * maxDragDistance;

        chargeSlider.value = Mathf.Clamp(dragDistance / maxDragDistance, 0, 1);

        Vector3 lookDirection = new Vector3(-currentDragPosition.x, 0, -currentDragPosition.y);
        if (lookDirection.magnitude > 0.1f)
        {
            character.forward = lookDirection.normalized;
        }
    }

    private void OnTouchEnd()
    {
        joystickUI.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;

        characterAnimator.SetBool("IsCharging", false);
        characterAnimator.SetTrigger("Attack");

        float force = chargeSlider.value * maxForce;
        Vector3 launchDirection = character.forward * force;
        characterRigidbody.AddForce(launchDirection, ForceMode.Impulse);
    }

    private void HandleEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Transform enemy = enemies[i];
            if (enemy != null)
            {
                Vector3 directionToPlayer = (character.position - enemy.position).normalized;
                float distanceToPlayer = Vector3.Distance(character.position, enemy.position);

                // 적이 플레이어를 바라보게 설정
                if (directionToPlayer.magnitude > 0.1f)
                {
                    enemy.forward = directionToPlayer;
                }

                // 적이 공격 범위 밖에서는 플레이어에게 밀리지 않음
                if (distanceToPlayer > attackRange)
                {
                    enemy.position += directionToPlayer * Time.deltaTime * enemySpeed;
                }

                enemyAttackTimers[i] -= Time.deltaTime;
                if (distanceToPlayer <= attackRange && enemyAttackTimers[i] <= 0f)
                {
                    AttackPlayer(enemy);
                    enemyAttackTimers[i] = enemyAttackInterval;
                }
            }
        }
    }

    private void AttackPlayer(Transform enemy)
    {
        // 플레이어를 향한 충격 방향 계산
        Vector3 knockbackDirection = (character.position - enemy.position).normalized;
        characterRigidbody.AddForce(knockbackDirection * playerKnockbackForce, ForceMode.Impulse); // 플레이어를 밀어냄
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisiong : " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                // 게이지 값 기반으로 강도 조정
                float effectiveKnockbackForce = knockbackForce * chargeSlider.value;
                Debug.Log("chargeSlider.value : " + chargeSlider.value);
                Debug.Log("effectiveKnockbackForce : " + effectiveKnockbackForce);
                Vector3 knockbackDirection = (enemyRigidbody.position - character.position).normalized;
                enemyRigidbody.AddForce(knockbackDirection * effectiveKnockbackForce, ForceMode.Impulse); // 적이 밀려남
            }
        }
    }
}
