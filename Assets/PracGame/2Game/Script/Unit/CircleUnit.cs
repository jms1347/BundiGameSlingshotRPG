// 파일명: CircleUnit.cs

using UnityEngine;

public class CircleUnit : MonoBehaviour // 이 부분을 변경
{
    // HP와 MP 게이지 컨트롤러를 인스펙터에서 할당
    public GaugeController hpGaugeController;
    public GaugeController mpGaugeController;

    [Header("Current Stats")]
    // [Range]를 사용해 인스펙터에서 값을 조절할 수 있게 합니다.
    [SerializeField, Range(0, 100)]
    private float currentHp;
    [SerializeField, Range(0, 100)]
    private float currentMp;

    [Header("Max Stats")]
    [SerializeField]
    private float maxHp = 100f;
    [SerializeField]
    private float maxMp = 50f;

    void Start()
    {
        // 게임 시작 시 현재 스탯을 최대값으로 초기화합니다.
        currentHp = maxHp;
        currentMp = maxMp;

        // 시작 값을 게이지에 바로 반영합니다.
        UpdateGauges();
    }

    void Update()
    {
        // 키보드 입력에 따른 HP, MP 변화 테스트 (예시)
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(5);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            UseMana(5);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ManaRegen(3);
        }
    }

    /// <summary>
    /// 플레이어의 체력을 감소시킵니다.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateGauges();
        Debug.Log("데미지 " + damageAmount + " 받음. 현재 HP: " + currentHp);
    }

    /// <summary>
    /// 플레이어의 체력을 회복시킵니다.
    /// </summary>
    public void Heal(float healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateGauges();
        Debug.Log("체력 " + healAmount + " 회복. 현재 HP: " + currentHp);
    }

    /// <summary>
    /// 플레이어의 마나를 소모합니다.
    /// </summary>
    public void UseMana(float manaCost)
    {
        currentMp -= manaCost;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        UpdateGauges();
        Debug.Log("마나 " + manaCost + " 사용. 현재 MP: " + currentMp);
    }

    /// <summary>
    /// 플레이어의 마나를 회복시킵니다.
    /// </summary>
    public void ManaRegen(float regenAmount)
    {
        currentMp += regenAmount;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        UpdateGauges();
        Debug.Log("마나 " + regenAmount + " 회복. 현재 MP: " + currentMp);
    }

    /// <summary>
    /// 현재 HP와 MP 값으로 게이지 UI를 업데이트합니다.
    /// </summary>
    private void UpdateGauges()
    {
        if (hpGaugeController != null)
        {
            // 정규화된 HP 값을 계산하여 컨트롤러에 전달합니다.
            float normalizedHp = currentHp / maxHp;
            hpGaugeController.SetFillAmount(normalizedHp);
        }

        if (mpGaugeController != null)
        {
            // 정규화된 MP 값을 계산하여 컨트롤러에 전달합니다.
            float normalizedMp = currentMp / maxMp;
            mpGaugeController.SetFillAmount(normalizedMp);
        }
    }
}