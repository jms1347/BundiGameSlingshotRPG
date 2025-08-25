using UnityEngine;

using UnityEngine;

//public class UnitStatsController : MonoBehaviour
//{
//    // HP�� MP ������ ��Ʈ�ѷ� (UI ������Ʈ)
//    public GaugeController hpGaugeController;
//    public GaugeController mpGaugeController;

//    // ������ ���� ������Ʈ
//    public Unit unit;

//    public void SetUnitData(Unit pUnit)
//    {
//        unit = pUnit;
//    }
//    // �� ������ UI�� ������Ʈ
//    private void Update()
//    {
//        // ������ stats ������ �ִٸ�
//        if (unit != null)
//        {
//            UpdateHpGauge(unit.Stats.CurrentHp, unit.Stats.MaxHp);
//            UpdateMpGauge(unit.Stats.CurrentMp, unit.Stats.MaxMp);
//        }
//    }



//}
public class UnitStatsController : MonoBehaviour
{
    // HP�� MP ������ ��Ʈ�ѷ��� �ν����Ϳ��� �Ҵ�
    public GaugeController hpGaugeController;
    public GaugeController mpGaugeController;

    // ������ ���� ������Ʈ
    public Unit unit;

    public void SetUnitData(Unit pUnit)
    {
        unit = pUnit;
    }


    void Update()
    {

        if (unit != null)
        {
            UpdateHpGauge(unit.Stats.CurrentHp, unit.Stats.MaxHp);
            UpdateMpGauge(unit.Stats.CurrentMp, unit.Stats.MaxMp);
        }

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
        unit.Stats.CurrentHp -= damageAmount;
        unit.Stats.CurrentHp = Mathf.Clamp(unit.Stats.CurrentHp, 0, unit.Stats.MaxHp);
        UpdateGauges();
        Debug.Log("������ " + damageAmount + " ����. ���� HP: " + unit.Stats.CurrentHp);
    }

    /// <summary>
    /// �÷��̾��� ü���� ȸ����ŵ�ϴ�.
    /// </summary>
    public void Heal(float healAmount)
    {
        unit.Stats.CurrentHp += healAmount;
        unit.Stats.CurrentHp = Mathf.Clamp(unit.Stats.CurrentHp, 0, unit.Stats.MaxHp);
        UpdateGauges();
        Debug.Log("ü�� " + healAmount + " ȸ��. ���� HP: " + unit.Stats.CurrentHp);
    }

    /// <summary>
    /// �÷��̾��� ������ �Ҹ��մϴ�.
    /// </summary>
    public void UseMana(float manaCost)
    {
        unit.Stats.CurrentMp -= manaCost;
        unit.Stats.CurrentMp = Mathf.Clamp(unit.Stats.CurrentMp, 0, unit.Stats.MaxMp); ;
        UpdateGauges();
        Debug.Log("���� " + manaCost + " ���. ���� MP: " + unit.Stats.CurrentMp);
    }

    /// <summary>
    /// �÷��̾��� ������ ȸ����ŵ�ϴ�.
    /// </summary>
    public void ManaRegen(float regenAmount)
    {
        unit.Stats.CurrentMp += regenAmount;
        unit.Stats.CurrentMp = Mathf.Clamp(unit.Stats.CurrentMp, 0, unit.Stats.MaxMp);
        UpdateGauges();
        Debug.Log("���� " + regenAmount + " ȸ��. ���� MP: " + unit.Stats.CurrentMp);
    }

    /// <summary>
    /// ���� HP�� MP ������ ������ UI�� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateGauges()
    {
        if (hpGaugeController != null)
        {
            // ����ȭ�� HP ���� ����Ͽ� ��Ʈ�ѷ��� �����մϴ�.
            float normalizedHp = unit.Stats.CurrentHp / unit.Stats.MaxHp;
            hpGaugeController.SetFillAmount(normalizedHp);
        }

        if (mpGaugeController != null)
        {
            // ����ȭ�� MP ���� ����Ͽ� ��Ʈ�ѷ��� �����մϴ�.
            float normalizedMp = unit.Stats.CurrentMp / unit.Stats.MaxMp;
            mpGaugeController.SetFillAmount(normalizedMp);
        }
    }

    private void UpdateHpGauge(float current, float max)
    {
        if (hpGaugeController != null)
        {
            float normalizedValue = current / max;
            hpGaugeController.SetFillAmount(normalizedValue);
        }
    }

    private void UpdateMpGauge(float current, float max)
    {
        if (mpGaugeController != null)
        {
            float normalizedValue = current / max;
            mpGaugeController.SetFillAmount(normalizedValue);
        }
    }
}
