using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    IEnumerator countdownCour;
    [SerializeField] private bool isClickStartBtn = false;

    private void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        if (countdownCour != null)
            StopCoroutine(countdownCour);
        countdownCour = CountDownCour();
        StartCoroutine(countdownCour);
    }

    public void StartCountDown(string pText, float pEndScale = 1.2f)
    {
        countdownText.color = new Color32(255, 255, 255, 0);
        countdownText.transform.localScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one * pEndScale;
        //countdownText.canvasRenderer.SetAlpha(0f); // 텍스트 페이드를 위해 알파값 초기화

        countdownText.text = pText;
        countdownText.transform.DOScale(endScale, 1).SetEase(Ease.OutQuad); // 스케일 애니메이션

        // 페이드 애니메이션
        countdownText.DOFade(1f, 0.5f).SetEase(Ease.OutQuad); // 점점 나타남
        countdownText.DOFade(0f, 0.5f).SetDelay(0.5f); // 점점 사라짐
    }

    public IEnumerator CountDownCour()
    {
        SetActiveCountDown(true);

        countdownText.text = "";
        yield return new WaitUntil(() => isClickStartBtn);

        StartCountDown("3");
        //SoundManager.instance.PlayCreateSfx(SoundManager.SoundType.UI, SoundKeyStringUtils.GetSoundKeyString(SoundKeyStringUtils.SoundNameKey.CountDown));
        yield return Utils.WaitForSecond(1.0f);
        //SoundManager.instance.PlayCreateSfx(SoundManager.SoundType.UI, SoundKeyStringUtils.GetSoundKeyString(SoundKeyStringUtils.SoundNameKey.CountDown));
        StartCountDown("2");
        yield return Utils.WaitForSecond(1.0f);
        //SoundManager.instance.PlayCreateSfx(SoundManager.SoundType.UI, SoundKeyStringUtils.GetSoundKeyString(SoundKeyStringUtils.SoundNameKey.CountDown));
        StartCountDown("1");
        yield return Utils.WaitForSecond(1.0f);
        //SoundManager.instance.PlayCreateSfx(SoundManager.SoundType.UI, SoundKeyStringUtils.GetSoundKeyString(SoundKeyStringUtils.SoundNameKey.CountDown));
        StartCountDown("시작!", 1.0f);

        yield return Utils.WaitForSecond(1.0f);
        countdownText.gameObject.SetActive(false);
        SetActiveCountDown(false);
    }


    #region 튜토리얼 팝업 세팅
    public void SetActiveCountDown(bool pBool)
    {
        //SoundManager.instance.PlayClickSFX1();

        this.gameObject.SetActive(pBool);
    }
    #endregion

    #region 
    public void StartBtn()
    {
        //SoundManager.instance.PlaySFXByKey("click1");
        isClickStartBtn = true;
    }
    #endregion


    #region 체크 튜토리얼 여부
    public bool CheckEndCountDown()
    {
        return isClickStartBtn;
    }

    public void StartCountDown()
    {
        isClickStartBtn = true;
    }
    #endregion
}