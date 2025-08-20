using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private Material spriteMaterial;

    // �ν����Ϳ��� �巡���ؼ� �־��ּ���
    public SpriteRenderer spriteRenderer;

    // ���� ü�� �� (0.0f ~ 1.0f)
    [Range(0f, 1f)]
    public float fillAmount = 1.0f;

    void Start()
    {
        // SpriteRenderer�� ������ �����ؼ� ����ؾ� ������ �������� �ʾ�
        spriteMaterial = spriteRenderer.material;
    }

    void Update()
    {
        // ���̴��� "_FillAmount" �Ӽ� ���� ������Ʈ
        spriteMaterial.SetFloat("_FillAmount", fillAmount);
    }
}