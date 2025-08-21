using UnityEngine;
using UnityEditor;

public class RightHalfGaugeController : MonoBehaviour
{
    private Material gaugeMaterial;
    private SpriteRenderer spriteRenderer;

    // 테스트용 변수
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
    /// 오른쪽 반원 게이지 채우기
    /// </summary>
    /// <param name="amount">0.0f에서 1.0f 사이의 값</param>
    public void SetFillAmount(float amount)
    {
        if (gaugeMaterial != null)
        {
            gaugeMaterial.SetFloat("_FillAmount", Mathf.Clamp01(amount));
        }
    }

    // 에디터에서만 실행되는 테스트 함수
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