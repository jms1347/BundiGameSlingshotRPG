using System;
using UnityEngine;


// Character.cs
public class Character : MonoBehaviour, IHealthAffected
{
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;

    [Header("캐릭터 기본 능력치")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float currentHp = 100f;
    [SerializeField] private float maxMp = 100f;
    [SerializeField] private float currentMp = 100f;


    // TODO: Animator 컴포넌트 참조 (애니메이션 재생용)
    protected Animator _animator;

    // Character가 살아있는지 여부 (예시)
    public bool IsAlive => CurrentHp > 0;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp
    {
        get => currentHp;
        set
        {
            // Only update if the value actually changes to avoid unnecessary event calls
            if (currentHp != value)
            {
                currentHp = value;
                // Invoke the event whenever CurrentHp is set
                OnHealthChanged?.Invoke(currentHp, MaxHp);
            }
        }
    }
    public float MaxMp { get => maxMp; set => maxMp = value; }
    public float CurrentMp
    {
        get => currentMp;
        set
        {
            // Only update if the value actually changes
            if (currentMp != value)
            {
                currentMp = value;
                // Invoke the event whenever CurrentMp is set
                OnManaChanged?.Invoke(currentMp, MaxMp);
            }
        }
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 참조

        OnHealthChanged?.Invoke(CurrentHp, MaxHp);
        OnManaChanged?.Invoke(CurrentMp, MaxMp);
    }

    private void Start()
    {
        TakeDamage(20);
        TakeHeal(10);
        TryConsumeMana(15);
    }

    protected virtual void Update()
    {
        // 체력/마나 회복 로직 등은 여전히 Update에서 처리할 수 있습니다.
        // 예를 들어, 초당 1씩 회복하고 싶다면:
        // CurrentMana.Value = Mathf.Min(_maxMana, CurrentMana.Value + Time.deltaTime);
    }

    // 스킬 사용 시 마나를 소모하는 메서드.
    public bool TryConsumeMana(float amount)
    {
        if (CurrentMp >= amount)
        {
            CurrentMp -= amount; // This will trigger the OnManaChanged event via the property setter
            Debug.Log($"{name}이(가) 마나 {amount} 소모. 남은 마나: {CurrentMp}");
            return true;
        }
        Debug.LogWarning($"{name} 마나 부족! (필요: {amount}, 현재: {CurrentMp})");
        return false;
    }

    // 캐릭터 사망 처리.
    protected virtual void Die()
    {
        Debug.Log($"{name}이(가) 사망했습니다.");
        // TODO: 사망 애니메이션, 오브젝트 비활성화/파괴 등
        // Destroy(gameObject);
        // gameObject.SetActive(false);
    }

    // 스킬 발동 시 애니메이션을 트리거하는 메서드.
    public void PlayAnimation(string animTriggerName)
    {
        if (_animator != null)
        {
            _animator.SetTrigger(animTriggerName);
            Debug.Log($"{name}: '{animTriggerName}' 애니메이션 재생");
        }
        else
        {
            Debug.LogWarning($"'{name}'에 Animator 컴포넌트가 없습니다. 애니메이션 재생 불가: {animTriggerName}");
        }
    }
    #region IHeathAffected.cs 인터페이스 함수 구현
    public void TakeDamage(float amount)
    {
        CurrentHp -= amount; // This will trigger the OnHealthChanged event via the property setter
        if (CurrentHp < 0) CurrentHp = 0; // Ensure health doesn't go below zero

        Debug.Log($"{name}이(가) {amount} 피해를 입어 현재 체력: {CurrentHp}");
    }

    public void TakeHeal(float amount)
    {
        CurrentHp += amount; // This will trigger the OnHealthChanged event via the property setter
        if (CurrentHp > MaxHp) CurrentHp = MaxHp; // Ensure health doesn't exceed max

        Debug.Log($"{name}이(가) {amount} 힐을 받아 현재 체력: {CurrentHp}");
    }
    #endregion
    // 추가적인 캐릭터 공통 로직 (이동, 상태 이상 등)
}