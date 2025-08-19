using UnityEngine;
using DG.Tweening;

public class PopupFade : MonoBehaviour
{
    public float duration = 0.5f; // 애니메이션 지속 시간
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        this.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
    }

    public void OpenPopup()
    {
        SoundManager.Instance.PlayCreateSfx(SoundManager.SoundType.Sfx, "FadePop");

        canvasGroup.alpha = 0; // 초기 투명도 설정

        this.gameObject.SetActive(true);
        canvasGroup.DOFade(1, duration).SetEase(Ease.OutQuad);
    }

    public void ClosePopup()
    {
        SoundManager.Instance.PlayCreateSfx(SoundManager.SoundType.Sfx, "PopupClose");

        canvasGroup.DOFade(0, duration).SetEase(Ease.InQuad).OnComplete(() => {
            this.gameObject.SetActive(false);
        });

    }
}