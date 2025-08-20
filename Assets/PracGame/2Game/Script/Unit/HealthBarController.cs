using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private Material spriteMaterial;

    // 인스펙터에서 드래그해서 넣어주세요
    public SpriteRenderer spriteRenderer;

    // 현재 체력 값 (0.0f ~ 1.0f)
    [Range(0f, 1f)]
    public float fillAmount = 1.0f;

    void Start()
    {
        // SpriteRenderer의 재질을 복사해서 사용해야 재질이 공유되지 않아
        spriteMaterial = spriteRenderer.material;
    }

    void Update()
    {
        // 셰이더의 "_FillAmount" 속성 값을 업데이트
        spriteMaterial.SetFloat("_FillAmount", fillAmount);
    }
}