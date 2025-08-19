using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    [SerializeField] private Unit targetUnit; // �� UI�� ����ٴ� ����

    [Header("HP Bar Sprites")]
    // ü�·��� ��Ÿ�� ��������Ʈ ������ (�ǹ��� ���� �߾� ����)
    [SerializeField] private SpriteRenderer hpBarFilledSprite;
    // ü�� ���� ��� (������ ����)
    [SerializeField] private SpriteRenderer hpBarBackgroundSprite;

    // HP ���� �ִ� ���� (Filled Sprite�� �ʱ� localScale.x ��)
    private float maxHpBarWidth;

    private void Awake()
    {
        if (targetUnit == null)
        {
            Debug.LogError("Target Unit not assigned to UnitUIController.", this);
            enabled = false;
            return;
        }

        // HP �� Filled Sprite�� �ʱ� ������.x ���� �����صӴϴ�.
        // �� ���� HP �ٰ� �� á�� ���� ���̸� ��Ÿ���ϴ�.
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

        // UnitStats�� OnHealthChanged �̺�Ʈ�� �����մϴ�.
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
        // �ʱ� HP �� ���¸� �����մϴ�.
        if (targetUnit != null && targetUnit.Stats != null)
        {
            UpdateHealthBarSprite(targetUnit.Stats.CurrentHP, targetUnit.Stats.MaxHP);
        }
    }

    private void OnDestroy()
    {
        // ������Ʈ �ı� �� �̺�Ʈ ������ �����Ͽ� �޸� ������ �����մϴ�.
        if (targetUnit != null && targetUnit.Stats != null)
        {
            targetUnit.Stats.OnHealthChanged -= UpdateHealthBarSprite;
            Debug.Log($"UnitUIController unsubscribed from {targetUnit.name}'s OnHealthChanged event.");
        }
    }

    /// <summary>
    /// OnHealthChanged �̺�Ʈ�� �߻��� �� ȣ��Ǵ� �޼���.
    /// SpriteRenderer�� localScale.x�� �����Ͽ� HP �� ���̸� ������Ʈ�մϴ�.
    /// </summary>
    /// <param name="currentHp">���� ü��</param>
    /// <param name="maxHp">�ִ� ü��</param>
    private void UpdateHealthBarSprite(float currentHp, float maxHp)
    {
        if (hpBarFilledSprite == null) return;

        // ü�� ������ ����մϴ� (0 ~ 1 ���� ��)
        float healthRatio = maxHp > 0 ? currentHp / maxHp : 0;

        // ���� ������ ���� HP ���� X �������� �����մϴ�.
        // Y�� Z �������� �������� �ʽ��ϴ�.
        hpBarFilledSprite.transform.localScale = new Vector3(
            maxHpBarWidth * healthRatio, // HP ���� �ʺ� ü�� ������ �°� ����
            hpBarFilledSprite.transform.localScale.y,
            hpBarFilledSprite.transform.localScale.z
        );

        // Debug.Log($"Updating HP bar for {targetUnit.name}: {currentHp}/{maxHp}, Scale.x: {hpBarFilledSprite.transform.localScale.x}");
    }
}
