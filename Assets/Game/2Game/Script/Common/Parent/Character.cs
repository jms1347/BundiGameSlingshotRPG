using System;
using UnityEngine;


// Character.cs
public class Character : MonoBehaviour, IHealthAffected
{
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;

    [Header("ĳ���� �⺻ �ɷ�ġ")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float currentHp = 100f;
    [SerializeField] private float maxMp = 100f;
    [SerializeField] private float currentMp = 100f;


    // TODO: Animator ������Ʈ ���� (�ִϸ��̼� �����)
    protected Animator _animator;

    // Character�� ����ִ��� ���� (����)
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
        _animator = GetComponent<Animator>(); // Animator ������Ʈ ����

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
        // ü��/���� ȸ�� ���� ���� ������ Update���� ó���� �� �ֽ��ϴ�.
        // ���� ���, �ʴ� 1�� ȸ���ϰ� �ʹٸ�:
        // CurrentMana.Value = Mathf.Min(_maxMana, CurrentMana.Value + Time.deltaTime);
    }

    // ��ų ��� �� ������ �Ҹ��ϴ� �޼���.
    public bool TryConsumeMana(float amount)
    {
        if (CurrentMp >= amount)
        {
            CurrentMp -= amount; // This will trigger the OnManaChanged event via the property setter
            Debug.Log($"{name}��(��) ���� {amount} �Ҹ�. ���� ����: {CurrentMp}");
            return true;
        }
        Debug.LogWarning($"{name} ���� ����! (�ʿ�: {amount}, ����: {CurrentMp})");
        return false;
    }

    // ĳ���� ��� ó��.
    protected virtual void Die()
    {
        Debug.Log($"{name}��(��) ����߽��ϴ�.");
        // TODO: ��� �ִϸ��̼�, ������Ʈ ��Ȱ��ȭ/�ı� ��
        // Destroy(gameObject);
        // gameObject.SetActive(false);
    }

    // ��ų �ߵ� �� �ִϸ��̼��� Ʈ�����ϴ� �޼���.
    public void PlayAnimation(string animTriggerName)
    {
        if (_animator != null)
        {
            _animator.SetTrigger(animTriggerName);
            Debug.Log($"{name}: '{animTriggerName}' �ִϸ��̼� ���");
        }
        else
        {
            Debug.LogWarning($"'{name}'�� Animator ������Ʈ�� �����ϴ�. �ִϸ��̼� ��� �Ұ�: {animTriggerName}");
        }
    }
    #region IHeathAffected.cs �������̽� �Լ� ����
    public void TakeDamage(float amount)
    {
        CurrentHp -= amount; // This will trigger the OnHealthChanged event via the property setter
        if (CurrentHp < 0) CurrentHp = 0; // Ensure health doesn't go below zero

        Debug.Log($"{name}��(��) {amount} ���ظ� �Ծ� ���� ü��: {CurrentHp}");
    }

    public void TakeHeal(float amount)
    {
        CurrentHp += amount; // This will trigger the OnHealthChanged event via the property setter
        if (CurrentHp > MaxHp) CurrentHp = MaxHp; // Ensure health doesn't exceed max

        Debug.Log($"{name}��(��) {amount} ���� �޾� ���� ü��: {CurrentHp}");
    }
    #endregion
    // �߰����� ĳ���� ���� ���� (�̵�, ���� �̻� ��)
}