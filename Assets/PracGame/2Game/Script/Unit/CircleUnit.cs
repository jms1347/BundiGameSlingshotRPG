// ���ϸ�: CircleUnit.cs

using UnityEngine;

public class CircleUnit : MonoBehaviour // �� �κ��� ����
{
    // HP�� MP ������ ��Ʈ�ѷ��� �ν����Ϳ��� �Ҵ�
    public GaugeController hpGaugeController;
    public GaugeController mpGaugeController;

    [Header("Current Stats")]
    // [Range]�� ����� �ν����Ϳ��� ���� ������ �� �ְ� �մϴ�.
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
        // ���� ���� �� ���� ������ �ִ밪���� �ʱ�ȭ�մϴ�.
        currentHp = maxHp;
        currentMp = maxMp;

        // ���� ���� �������� �ٷ� �ݿ��մϴ�.
        UpdateGauges();
    }

    void Update()
    {
        // Ű���� �Է¿� ���� HP, MP ��ȭ �׽�Ʈ (����)
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
    /// �÷��̾��� ü���� ���ҽ�ŵ�ϴ�.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateGauges();
        Debug.Log("������ " + damageAmount + " ����. ���� HP: " + currentHp);
    }

    /// <summary>
    /// �÷��̾��� ü���� ȸ����ŵ�ϴ�.
    /// </summary>
    public void Heal(float healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateGauges();
        Debug.Log("ü�� " + healAmount + " ȸ��. ���� HP: " + currentHp);
    }

    /// <summary>
    /// �÷��̾��� ������ �Ҹ��մϴ�.
    /// </summary>
    public void UseMana(float manaCost)
    {
        currentMp -= manaCost;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        UpdateGauges();
        Debug.Log("���� " + manaCost + " ���. ���� MP: " + currentMp);
    }

    /// <summary>
    /// �÷��̾��� ������ ȸ����ŵ�ϴ�.
    /// </summary>
    public void ManaRegen(float regenAmount)
    {
        currentMp += regenAmount;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        UpdateGauges();
        Debug.Log("���� " + regenAmount + " ȸ��. ���� MP: " + currentMp);
    }

    /// <summary>
    /// ���� HP�� MP ������ ������ UI�� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateGauges()
    {
        if (hpGaugeController != null)
        {
            // ����ȭ�� HP ���� ����Ͽ� ��Ʈ�ѷ��� �����մϴ�.
            float normalizedHp = currentHp / maxHp;
            hpGaugeController.SetFillAmount(normalizedHp);
        }

        if (mpGaugeController != null)
        {
            // ����ȭ�� MP ���� ����Ͽ� ��Ʈ�ѷ��� �����մϴ�.
            float normalizedMp = currentMp / maxMp;
            mpGaugeController.SetFillAmount(normalizedMp);
        }
    }
}