using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    Vector3 oriPos;

    private void Awake()
    {
        oriPos = this.transform.position;
    }

    private void Start()
    {
        //InitTooltip();
    }
    public void InitTooltip()
    {
        this.transform.position = oriPos;
        contentText.text = "";
    }

    public void OpenTooltip(string pContent, Vector3 pTooltipPos, float pTime = 2.0f)
    {
        this.transform.localPosition = pTooltipPos;
        contentText.text = pContent;
        OpenTooltipAni(pTime);
    }
    public void OpenTooltip(string pContent, float pTime = 2.0f)
    {
        contentText.text = pContent;
        OpenTooltipAni(pTime);
    }
    public void CloseToolTip()
    {
        CloseToolTipAni();
    }

    #region 애니메이션 효과 함수
    public void OpenTooltipAni(float pTime)
    {
        this.transform.localScale = Vector3.one * 0.1f;
        this.gameObject.SetActive(true);
        this.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() => {
            this.transform.DOScale(Vector3.one, 0.1f);
            Invoke(nameof(CloseToolTip), pTime);
        });
    }
    public void CloseToolTipAni()
    {
        this.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => {
            this.transform.DOScale(Vector3.one * 0.1f, 0.2f).OnComplete(() => {
                this.gameObject.SetActive(false);
            });
        });
    }
    #endregion
}

