using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupPrefab
{
    public string popupKey;
    public GameObject popup;
}


public class PopupData
{
    public string titleStr;
    public string contentStr;
    public string oneBtnStr;
    public string twoBtnStr;
}

public class PopupManager : Singleton<PopupManager>
{
    [Header("기본 메시지 팝업")]
    public List<PopupPrefab> popupPrefabList; // Inspector에서 할당
    public Transform popupParent;
    public GameObject finalOpenPop;

    [SerializeField] private List<Popup> onePopupList = new List<Popup>();
    [SerializeField] private List<Popup> twoPopupList = new List<Popup>();

    [Header("툴팁")]
    [SerializeField] private List<Tooltip> tooltipList = new List<Tooltip>();



    #region 팝업 관련 함수
    public void InitPopupList()
    {
        CloseAllPopup();
    }

    //예시
    public void ExPopupOpen()
    {
        PopupData tempData = new PopupData();
        tempData.titleStr = "입력한 데이터가 초기화됩니다.";
        tempData.contentStr = "초기화 된 데이터는 복구할 수 없어요.";
        tempData.oneBtnStr = "취소";
        tempData.twoBtnStr = "확인";

        PopupManager.Instance.OpenPopup(tempData, () => { }, () => { });
    }
    #region SetPopup 종류

    public void OpenPopup(PopupData pData, Action OneBtnFun)
    {
        for (int i = 0; i < onePopupList.Count; i++)
        {
            if (!onePopupList[i].gameObject.activeSelf)
            {
                finalOpenPop = onePopupList[i].gameObject;
                onePopupList[i].SetPopup(pData.titleStr, pData.contentStr, OneBtnFun);
                onePopupList[i].transform.SetAsLastSibling();
                break;
            }
        }
    }
    public void OpenPopup(PopupData pData, Action OneBtnFun, Action TwoBtnFun)
    {
        for (int i = 0; i < twoPopupList.Count; i++)
        {
            if (!twoPopupList[i].gameObject.activeSelf)
            {
                finalOpenPop = twoPopupList[i].gameObject;
                twoPopupList[i].SetPopup(pData.titleStr, pData.contentStr, OneBtnFun, TwoBtnFun);
                twoPopupList[i].transform.SetAsLastSibling();
                break;
            }
        }
    }

    // 2버튼 팝업 세팅
    public void OpenPopup(string pTitle, string pContent, Action OneBtnFun, Action TwoBtnFun)
    {
        for (int i = 0; i < twoPopupList.Count; i++)
        {
            if (!twoPopupList[i].gameObject.activeSelf)
            {
                finalOpenPop = twoPopupList[i].gameObject;
                twoPopupList[i].SetPopup(pTitle, pContent, OneBtnFun, TwoBtnFun);
                twoPopupList[i].transform.SetAsLastSibling();
                break;
            }
        }
    }

    //1버튼 팝업 세팅
    public void OpenPopup(string pTitle, string pContent, Action OneBtnFun)
    {
        for (int i = 0; i < onePopupList.Count; i++)
        {
            if (!onePopupList[i].gameObject.activeSelf)
            {
                finalOpenPop = onePopupList[i].gameObject;
                onePopupList[i].SetPopup(pTitle, pContent, OneBtnFun);
                onePopupList[i].transform.SetAsLastSibling();
                break;
            }
        }
    }
    #endregion
    #region 전체 팝업 닫기
    public void CloseAllPopup()
    {
        //열린것만 닫게하려면 열때마다 스택이나 큐에 보관해야될 듯
        for (int i = 0; i < onePopupList.Count; i++)
        {
            onePopupList[i].ClosePopup();
        }
        for (int i = 0; i < twoPopupList.Count; i++)
        {
            twoPopupList[i].ClosePopup();
        }
    }

    public void CloseFinalPopup()
    {
        //열린 순으로 닫게하려면 열때마다 스택이나 큐에 보관해야될 듯 (일단 마지막 것만 관리)

        finalOpenPop?.SetActive(false);
        finalOpenPop = null;
    }
    #endregion
    #endregion

    #region 툴팁 관련 함수
    public void InitTooltipList()
    {
        for (int i = 0; i < tooltipList.Count; i++)
        {
            tooltipList[i].InitTooltip();
        }
    }

    #region 툴팁 열기
    public void OpenTooltip(string pContent, Vector3 pTooltipPos, float pTime = 2.0f)
    {
        for (int i = 0; i < tooltipList.Count; i++)
        {
            if (!tooltipList[i].gameObject.activeSelf)
            {
                tooltipList[i].OpenTooltip(pContent, pTooltipPos, pTime);
                break;
            }
        }
    }
    public void OpenTooltip(string pContent, float pTime = 2.0f)
    {
        for (int i = 0; i < tooltipList.Count; i++)
        {
            if (!tooltipList[i].gameObject.activeSelf)
            {
                tooltipList[i].OpenTooltip(pContent, pTime);
                break;
            }
        }
    }
    #endregion
    #endregion



    #region 애니메이션 효과 함수
    public void OpenAni(GameObject pPopup, GameObject pAniPopup)
    {
        pAniPopup.transform.localScale = Vector3.one * 0.1f;
        pPopup.SetActive(true);
        pAniPopup.transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() => {
            pAniPopup.transform.DOScale(Vector3.one, 0.1f);
        });
    }
    public void CloseAni(GameObject pPopup, GameObject pAniPopup)
    {
        pAniPopup.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => {
            pAniPopup.transform.DOScale(Vector3.one * 0.1f, 0.2f).OnComplete(() => {
                pPopup.gameObject.SetActive(false);
            });
        });
    }
    #endregion
}
