using UnityEngine;
using System.Collections; // �ڷ�ƾ�� ����ϱ� ���� �ʿ��մϴ�.

public class GaugeController : MonoBehaviour
{
    private Material gaugeMaterial;
    private SpriteRenderer spriteRenderer;

    // �������� ���� ��ǥ�� (HP/MP�� ���� ����)
    private float targetFill = 0.0f;
    // ���� UI�� ǥ�õǴ� �ε巯�� ������ ��
    private float currentVisibleFill = 0.0f;

    [Header("�ִϸ��̼� �ӵ�")]
    public float animationDuration = 0.5f; // �ִϸ��̼��� �Ϸ�Ǵ� �ð� (��)

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
        currentVisibleFill = targetFill; // ���� �� �ʱⰪ ����
    }

    /// <summary>
    /// HP/MP �ý��ۿ��� ȣ���ϴ� �Լ�. �������� ���� ��ǥ���� �����ϰ� �ִϸ��̼��� �����մϴ�.
    /// </summary>
    /// <param name="amount">0.0f���� 1.0f ������ ��ǥ��</param>
    public void SetFillAmount(float amount)
    {
        // ��ǥ��(targetFill)�� ����
        targetFill = Mathf.Clamp01(amount);

        // �̹� �ڷ�ƾ�� ���� ���̶�� �����ϰ� ���ο� �ִϸ��̼� ����
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

            // Lerp �Լ��� ����Ͽ� ���� ���� ��ǥ������ �ε巴�� ����
            currentVisibleFill = Mathf.Lerp(startFill, targetFill, lerpProgress);

            // �ε巴�� ���ϴ� ���� ���̴��� ����
            gaugeMaterial.SetFloat("_FillAmount", currentVisibleFill);

            yield return null; // ���� �����ӱ��� ���
        }

        // �ִϸ��̼� �Ϸ� �� ��Ȯ�� ���������� ����
        currentVisibleFill = targetFill;
        gaugeMaterial.SetFloat("_FillAmount", currentVisibleFill);
    }
}