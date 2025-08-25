using UnityEngine;

using UnityEngine;

//public class UnitStatsController : MonoBehaviour
//{
//    // HP와 MP 게이지 컨트롤러 (UI 컴포넌트)
//    public GaugeController hpGaugeController;
//    public GaugeController mpGaugeController;

//    // 제어할 유닛 컴포넌트
//    public Unit unit;

//    public void SetUnitData(Unit pUnit)
//    {
//        unit = pUnit;
//    }
//    // 매 프레임 UI를 업데이트
//    private void Update()
//    {
//        // 유닛의 stats 정보가 있다면
//        if (unit != null)
//        {
//            UpdateHpGauge(unit.Stats.CurrentHp, unit.Stats.MaxHp);
//            UpdateMpGauge(unit.Stats.CurrentMp, unit.Stats.MaxMp);
//        }
//    }



//}
public class UnitStatsController : MonoBehaviour
{
    // HP와 MP 게이지 컨트롤러를 인스펙터에서 할당
    public GaugeController hpGaugeController;
    public GaugeController mpGaugeController;

    // 제어할 유닛 컴포넌트
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
        unit.Stats.CurrentHp -= damageAmount;
        unit.Stats.CurrentHp = Mathf.Clamp(unit.Stats.CurrentHp, 0, unit.Stats.MaxHp);
        UpdateGauges();
        Debug.Log("데미지 " + damageAmount + " 받음. 현재 HP: " + unit.Stats.CurrentHp);
    }

    /// <summary>
    /// 플레이어의 체력을 회복시킵니다.
    /// </summary>
    public void Heal(float healAmount)
    {
        unit.Stats.CurrentHp += healAmount;
        unit.Stats.CurrentHp = Mathf.Clamp(unit.Stats.CurrentHp, 0, unit.Stats.MaxHp);
        UpdateGauges();
        Debug.Log("체력 " + healAmount + " 회복. 현재 HP: " + unit.Stats.CurrentHp);
    }

    /// <summary>
    /// 플레이어의 마나를 소모합니다.
    /// </summary>
    public void UseMana(float manaCost)
    {
        unit.Stats.CurrentMp -= manaCost;
        unit.Stats.CurrentMp = Mathf.Clamp(unit.Stats.CurrentMp, 0, unit.Stats.MaxMp); ;
        UpdateGauges();
        Debug.Log("마나 " + manaCost + " 사용. 현재 MP: " + unit.Stats.CurrentMp);
    }

    /// <summary>
    /// 플레이어의 마나를 회복시킵니다.
    /// </summary>
    public void ManaRegen(float regenAmount)
    {
        unit.Stats.CurrentMp += regenAmount;
        unit.Stats.CurrentMp = Mathf.Clamp(unit.Stats.CurrentMp, 0, unit.Stats.MaxMp);
        UpdateGauges();
        Debug.Log("마나 " + regenAmount + " 회복. 현재 MP: " + unit.Stats.CurrentMp);
    }

    /// <summary>
    /// 현재 HP와 MP 값으로 게이지 UI를 업데이트합니다.
    /// </summary>
    private void UpdateGauges()
    {
        if (hpGaugeController != null)
        {
            // 정규화된 HP 값을 계산하여 컨트롤러에 전달합니다.
            float normalizedHp = unit.Stats.CurrentHp / unit.Stats.MaxHp;
            hpGaugeController.SetFillAmount(normalizedHp);
        }

        if (mpGaugeController != null)
        {
            // 정규화된 MP 값을 계산하여 컨트롤러에 전달합니다.
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
