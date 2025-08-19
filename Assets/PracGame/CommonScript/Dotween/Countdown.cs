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
        //countdownText.canvasRenderer.SetAlpha(0f); // �ؽ�Ʈ ���̵带 ���� ���İ� �ʱ�ȭ

        countdownText.text = pText;
        countdownText.transform.DOScale(endScale, 1).SetEase(Ease.OutQuad); // ������ �ִϸ��̼�

        // ���̵� �ִϸ��̼�
        countdownText.DOFade(1f, 0.5f).SetEase(Ease.OutQuad); // ���� ��Ÿ��
        countdownText.DOFade(0f, 0.5f).SetDelay(0.5f); // ���� �����
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
        StartCountDown("����!", 1.0f);

        yield return Utils.WaitForSecond(1.0f);
        countdownText.gameObject.SetActive(false);
        SetActiveCountDown(false);
    }


    #region Ʃ�丮�� �˾� ����
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


    #region üũ Ʃ�丮�� ����
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