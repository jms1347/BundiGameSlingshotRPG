using UnityEngine;
using UnityEditor;

public class RightHalfGaugeController : MonoBehaviour
{
    private Material gaugeMaterial;
    private SpriteRenderer spriteRenderer;

    // �׽�Ʈ�� ����
    [Range(0, 1)]
    public float testFillAmount = 1.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject.");
            return;
        }

        gaugeMaterial = spriteRenderer.material;
    }

    /// <summary>
    /// ������ �ݿ� ������ ä���
    /// </summary>
    /// <param name="amount">0.0f���� 1.0f ������ ��</param>
    public void SetFillAmount(float amount)
    {
        if (gaugeMaterial != null)
        {
            gaugeMaterial.SetFloat("_FillAmount", Mathf.Clamp01(amount));
        }
    }

    // �����Ϳ����� ����Ǵ� �׽�Ʈ �Լ�
    [ContextMenu("Apply Test Fill")]
    private void ApplyTestFill()
    {
        SetFillAmount(testFillAmount);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RightHalfGaugeController))]
    public class RightHalfGaugeControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            RightHalfGaugeController controller = (RightHalfGaugeController)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Apply Test Values"))
            {
                controller.SetFillAmount(controller.testFillAmount);
            }
        }
    }
#endif
}