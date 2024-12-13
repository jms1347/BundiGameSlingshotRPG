using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // 목표 크기
    public float duration = 1f; // 애니메이션 지속 시간

    private Vector3 oriScale;
    private void Awake()
    {
        oriScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }
    void Start()
    {
        // 애니메이션 시작
        AnimateScale();
    }

    void AnimateScale()
    {       
        // 현재 크기에서 목표 크기로 애니메이션
        transform.DOScale(targetScale, duration)
            .SetEase(Ease.InOutSine) // 애니메이션 이징 설정
            .OnComplete(() =>
            {
                // 목표 크기에서 원래 크기로 애니메이션
                transform.DOScale(oriScale, duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(AnimateScale); // 반복
            });
    }
}