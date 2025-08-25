using UnityEngine;
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

public class GaugeController : MonoBehaviour
{
    private Material gaugeMaterial;
    private SpriteRenderer spriteRenderer;

    // 게이지의 최종 목표값 (HP/MP의 실제 비율)
    private float targetFill = 0.0f;
    // 현재 UI에 표시되는 부드러운 게이지 값
    private float currentVisibleFill = 0.0f;

    [Header("애니메이션 속도")]
    public float animationDuration = 0.5f; // 애니메이션이 완료되는 시간 (초)

    private Coroutine fillCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject.");
            return;
        }
        gaugeMaterial = spriteRenderer.material;
        currentVisibleFill = targetFill; // 시작 시 초기값 설정
    }

    /// <summary>
    /// HP/MP 시스템에서 호출하는 함수. 게이지의 최종 목표값을 설정하고 애니메이션을 시작합니다.
    /// </summary>
    /// <param name="amount">0.0f에서 1.0f 사이의 목표값</param>
    public void SetFillAmount(float amount)
    {
        // 목표값(targetFill)만 변경
        targetFill = Mathf.Clamp01(amount);

        // 이미 코루틴이 실행 중이라면 중지하고 새로운 애니메이션 시작
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(AnimateGauge());
    }

    private IEnumerator AnimateGauge()
    {
        float startFill = currentVisibleFill;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpProgress = elapsedTime / animationDuration;

            // Lerp 함수를 사용하여 현재 값을 목표값까지 부드럽게 보간
            currentVisibleFill = Mathf.Lerp(startFill, targetFill, lerpProgress);

            // 부드럽게 변하는 값을 쉐이더에 전달
            gaugeMaterial.SetFloat("_FillAmount", currentVisibleFill);

            yield return null; // 다음 프레임까지 대기
        }

        // 애니메이션 완료 후 정확한 최종값으로 설정
        currentVisibleFill = targetFill;
        gaugeMaterial.SetFloat("_FillAmount", currentVisibleFill);
    }
}