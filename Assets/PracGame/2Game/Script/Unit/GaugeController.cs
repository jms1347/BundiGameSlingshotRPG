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
            // Material 인스턴스를 가져와서 사용해야 다른 오브젝트에 영향이 가지 않아요.
            gaugeMaterial = spriteRenderer.material;
        }
    }

    void Update()
    {
        if (gaugeMaterial != null)
        {
            // 스크립트에서 셰이더의 속성 값을 업데이트
            gaugeMaterial.SetFloat("_HPFillAmount", hpFillAmount);
            gaugeMaterial.SetFloat("_MPFillAmount", mpFillAmount);
        }
    }
}