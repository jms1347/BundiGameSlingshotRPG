using UnityEngine;
using UnityEngine.UI; // Required for Image
using TMPro;       // If using TextMeshPro
using System;      // Required for Action
using UniRx;
using DG.Tweening;

public class CharacterUIHandler : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Mana UI")]
    [SerializeField] private Image manaFillImage;
    [SerializeField] private TextMeshProUGUI manaText;

    public float healthDecreaseAnimationDuration = 0.3f;
    private Character targetCharacter;

    private void Start()
    {
    }

    public void SetTargetCharacter(Character character)
    {
        // First, unsubscribe from any previously linked character
        if (targetCharacter != null)
        {
            targetCharacter.OnHealthChanged -= UpdateHealthUI;
            targetCharacter.OnManaChanged -= UpdateManaUI;
        }

        targetCharacter = character;

        if (targetCharacter != null)
        {
            // Subscribe to the new character's events
            targetCharacter.OnHealthChanged += UpdateHealthUI;
            targetCharacter.OnManaChanged += UpdateManaUI;

            // Immediately update UI for initial state
            UpdateHealthUI(targetCharacter.CurrentHp, targetCharacter.MaxHp);
            UpdateManaUI(targetCharacter.CurrentMp, targetCharacter.MaxMp);
        }
    }

    // 즉시 UI를 업데이트하는 함수 (초기 설정용)
    private void SetHp()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = targetCharacter.CurrentHp / targetCharacter.MaxHp;
        }
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(targetCharacter.CurrentHp)} / {Mathf.CeilToInt(targetCharacter.MaxHp)}";
        }
    }

    private void UpdateHealthUI(float currentHp, float maxHp)
    {
        float targetFillAmount = currentHp / maxHp;
        if (healthFillImage != null)
        {
            healthFillImage.DOKill(); // Stop any ongoing animation
            healthFillImage.DOFillAmount(targetFillAmount, healthDecreaseAnimationDuration)
                .SetEase(Ease.OutCubic);
        }
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
        }
    }

    // This method is called when OnManaChanged event is invoked
    private void UpdateManaUI(float currentMp, float maxMp)
    {
        if (manaFillImage != null)
        {
            manaFillImage.fillAmount = currentMp / maxMp;
        }
        if (manaText != null)
        {
            manaText.text = $"{Mathf.CeilToInt(currentMp)} / {Mathf.CeilToInt(maxMp)}";
        }
    }
}