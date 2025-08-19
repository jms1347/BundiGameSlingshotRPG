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
    [Header("�⺻ �޽��� �˾�")]
    public List<PopupPrefab> popupPrefabList; // Inspector���� �Ҵ�
    public Transform popupParent;
    public GameObject finalOpenPop;

    [SerializeField] private List<Popup> onePopupList = new List<Popup>();
    [SerializeField] private List<Popup> twoPopupList = new List<Popup>();

    [Header("����")]
    [SerializeField] private List<Tooltip> tooltipList = new List<Tooltip>();



    #region �˾� ���� �Լ�
    public void InitPopupList()
    {
        CloseAllPopup();
    }

    //����
    public void ExPopupOpen()
    {
        PopupData tempData = new PopupData();
        tempData.titleStr = "�Է��� �����Ͱ� �ʱ�ȭ�˴ϴ�.";
        tempData.contentStr = "�ʱ�ȭ �� �����ʹ� ������ �� �����.";
        tempData.oneBtnStr = "���";
        tempData.twoBtnStr = "Ȯ��";

        PopupManager.Instance.OpenPopup(tempData, () => { }, () => { });
    }
    #region SetPopup ����

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

    // 2��ư �˾� ����
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

    //1��ư �˾� ����
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
    #region ��ü �˾� �ݱ�
    public void CloseAllPopup()
    {
        //�����͸� �ݰ��Ϸ��� �������� �����̳� ť�� �����ؾߵ� ��
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
        //���� ������ �ݰ��Ϸ��� �������� �����̳� ť�� �����ؾߵ� �� (�ϴ� ������ �͸� ����)

        finalOpenPop?.SetActive(false);
        finalOpenPop = null;
    }
    #endregion
    #endregion

    #region ���� ���� �Լ�
    public void InitTooltipList()
    {
        for (int i = 0; i < tooltipList.Count; i++)
        {
            tooltipList[i].InitTooltip();
        }
    }

    #region ���� ����
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



    #region �ִϸ��̼� ȿ�� �Լ�
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
