using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    [Header("버튼")]
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button dimmedBtn;

    [Header("실제 팝업")]
    [SerializeField] private GameObject pop;
    private void Awake()
    {
        confirmBtn?.onClick.AddListener(() => { ConfirmFun(); });
        cancelBtn?.onClick.AddListener(() => { CancelFun(); });
        dimmedBtn?.onClick.AddListener(() => { DimmedBtnFun(); });
    }

    public virtual void OpenPopup()
    {
        OpenAni();
    }

    public virtual void SetPopup(PopupData pData, Action OneBtnFun, Action TwoBtnFun)
    {
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        dimmedBtn.onClick.RemoveAllListeners();
        titleText.text = pData.titleStr;
        contentText.text = pData.contentStr;
        confirmBtn.GetComponent<TextMeshProUGUI>().text = pData.oneBtnStr;
        cancelBtn.GetComponent<TextMeshProUGUI>().text = pData.twoBtnStr;

        confirmBtn?.onClick.AddListener(() => {
            OneBtnFun();
            ClosePopup();
        });
        cancelBtn?.onClick.AddListener(() => {
            TwoBtnFun();
            ClosePopup();
        });

        OpenPopup();
    }

    public virtual void SetPopup(PopupData pData, Action OneBtnFun)
    {
        confirmBtn.onClick.RemoveAllListeners();

        titleText.text = pData.titleStr;
        contentText.text = pData.contentStr;
        confirmBtn.GetComponent<TextMeshProUGUI>().text = pData.oneBtnStr;

        confirmBtn?.onClick.AddListener(() => {
            OneBtnFun();
            ClosePopup();
        });
    }
    public virtual void SetPopup(string pTitle, string pContent, Action OneBtnFun, Action TwoBtnFun)
    {
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        dimmedBtn.onClick.RemoveAllListeners();
        titleText.text = pTitle;
        contentText.text = pContent;

        confirmBtn?.onClick.AddListener(() => {
            OneBtnFun();
            ClosePopup();
        });
        cancelBtn?.onClick.AddListener(() => {
            TwoBtnFun();
            ClosePopup();
        });

        OpenPopup();
    }

    public virtual void SetPopup(string pTitle, string pContent, Action OneBtnFun)
    {
        confirmBtn.onClick.RemoveAllListeners();

        titleText.text = pTitle;
        contentText.text = pContent;

        confirmBtn?.onClick.AddListener(() => {
            OneBtnFun();
            ClosePopup();
        });
    }

    public virtual void ClosePopup()
    {
        CloseAni();
    }
    #region 버튼 콜백
    public virtual void ConfirmFun()
    {
    }
    public virtual void CancelFun()
    {
        ClosePopup();
    }

    public virtual void DimmedBtnFun()
    {
        ClosePopup();
    }
    #endregion


    #region 애니메이션 효과 함수
    public void OpenAni()
    {
        pop.transform.localScale = Vector3.one * 0.1f;
        this.gameObject.SetActive(true);
        pop.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() => {
            pop.transform.DOScale(Vector3.one, 0.1f);
        });
    }
    public void CloseAni()
    {
        pop.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => {
            pop.transform.DOScale(Vector3.one * 0.1f, 0.2f).OnComplete(() => {
                this.gameObject.SetActive(false);
            });
        });
    }
    #endregion
    #region 애니메이션 툴팁 효과 함수
    public void OpenTooltipAni(float pTime)
    {
        pop.transform.localScale = Vector3.one * 0.1f;
        this.gameObject.SetActive(true);
        pop.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() => {
            pop.transform.DOScale(Vector3.one, 0.1f);
            Invoke(nameof(CloseToolTip), pTime);
        });
    }
    public void CloseToolTipAni()
    {
        pop.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => {
            pop.transform.DOScale(Vector3.one * 0.1f, 0.2f).OnComplete(() => {
                this.gameObject.SetActive(false);
            });
        });
    }

    public void CloseToolTip()
    {
        CloseToolTipAni();
    }
    #endregion
}