using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    [SerializeField] private Unit targetUnit; // 이 UI가 따라다닐 유닛

    [Header("HP Bar Sprites")]
    // 체력량을 나타낼 스프라이트 렌더러 (피벗은 왼쪽 중앙 권장)
    [SerializeField] private SpriteRenderer hpBarFilledSprite;
    // 체력 바의 배경 (고정된 길이)
    [SerializeField] private SpriteRenderer hpBarBackgroundSprite;

    // HP 바의 최대 길이 (Filled Sprite의 초기 localScale.x 값)
    private float maxHpBarWidth;

    private void Awake()
    {
        if (targetUnit == null)
        {
            Debug.LogError("Target Unit not assigned to UnitUIController.", this);
            enabled = false;
            return;
        }

        // HP 바 Filled Sprite의 초기 스케일.x 값을 저장해둡니다.
        // 이 값이 HP 바가 꽉 찼을 때의 길이를 나타냅니다.
        if (hpBarFilledSprite != null)
        {
            maxHpBarWidth = hpBarFilledSprite.transform.localScale.x;
        }
        else
        {
            Debug.LogError("HP Bar Filled SpriteRenderer not assigned!", this);
            enabled = false;
            return;
        }

        // UnitStats의 OnHealthChanged 이벤트를 구독합니다.
        if (targetUnit.Stats != null)
        {
            targetUnit.Stats.OnHealthChanged += UpdateHealthBarSprite;
            Debug.Log($"UnitUIController subscribed to {targetUnit.name}'s OnHealthChanged event.");
        }
        else
        {
            Debug.LogError($"Unit Stats not found for {targetUnit.name} to subscribe to health changes.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        // 초기 HP 바 상태를 설정합니다.
        if (targetUnit != null && targetUnit.Stats != null)
        {
            UpdateHealthBarSprite(targetUnit.Stats.CurrentHP, targetUnit.Stats.MaxHP);
        }
    }

    private void OnDestroy()
    {
        // 오브젝트 파괴 시 이벤트 구독을 해제하여 메모리 누수를 방지합니다.
        if (targetUnit != null && targetUnit.Stats != null)
        {
            targetUnit.Stats.OnHealthChanged -= UpdateHealthBarSprite;
            Debug.Log($"UnitUIController unsubscribed from {targetUnit.name}'s OnHealthChanged event.");
        }
    }

    /// <summary>
    /// OnHealthChanged 이벤트가 발생할 때 호출되는 메서드.
    /// SpriteRenderer의 localScale.x를 조절하여 HP 바 길이를 업데이트합니다.
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    /// <param name="maxHp">최대 체력</param>
    private void UpdateHealthBarSprite(float currentHp, float maxHp)
    {
        if (hpBarFilledSprite == null) return;

        // 체력 비율을 계산합니다 (0 ~ 1 사이 값)
        float healthRatio = maxHp > 0 ? currentHp / maxHp : 0;

        // 계산된 비율에 따라 HP 바의 X 스케일을 조절합니다.
        // Y와 Z 스케일은 변경하지 않습니다.
        hpBarFilledSprite.transform.localScale = new Vector3(
            maxHpBarWidth * healthRatio, // HP 바의 너비를 체력 비율에 맞게 조절
            hpBarFilledSprite.transform.localScale.y,
            hpBarFilledSprite.transform.localScale.z
        );

        // Debug.Log($"Updating HP bar for {targetUnit.name}: {currentHp}/{maxHp}, Scale.x: {hpBarFilledSprite.transform.localScale.x}");
    }
}
