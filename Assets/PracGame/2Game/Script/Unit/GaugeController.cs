using UnityEngine;

public class GaugeController : MonoBehaviour
{
    private Material gaugeMaterial;
    public SpriteRenderer spriteRenderer;

    [Header("HP Gauge")]
    [Range(0f, 1f)]
    public float hpFillAmount = 1.0f;

    [Header("MP Gauge")]
    [Range(0f, 1f)]
    public float mpFillAmount = 1.0f;

    void Start()
    {
        if (spriteRenderer != null)
        {
            // Material �ν��Ͻ��� �����ͼ� ����ؾ� �ٸ� ������Ʈ�� ������ ���� �ʾƿ�.
            gaugeMaterial = spriteRenderer.material;
        }
    }

    void Update()
    {
        if (gaugeMaterial != null)
        {
            // ��ũ��Ʈ���� ���̴��� �Ӽ� ���� ������Ʈ
            gaugeMaterial.SetFloat("_HPFillAmount", hpFillAmount);
            gaugeMaterial.SetFloat("_MPFillAmount", mpFillAmount);
        }
    }
}