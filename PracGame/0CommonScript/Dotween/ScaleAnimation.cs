using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // ��ǥ ũ��
    public float duration = 1f; // �ִϸ��̼� ���� �ð�

    private Vector3 oriScale;
    private void Awake()
    {
        oriScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }
    void Start()
    {
        // �ִϸ��̼� ����
        AnimateScale();
    }

    void AnimateScale()
    {       
        // ���� ũ�⿡�� ��ǥ ũ��� �ִϸ��̼�
        transform.DOScale(targetScale, duration)
            .SetEase(Ease.InOutSine) // �ִϸ��̼� ��¡ ����
            .OnComplete(() =>
            {
                // ��ǥ ũ�⿡�� ���� ũ��� �ִϸ��̼�
                transform.DOScale(oriScale, duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(AnimateScale); // �ݺ�
            });
    }
}