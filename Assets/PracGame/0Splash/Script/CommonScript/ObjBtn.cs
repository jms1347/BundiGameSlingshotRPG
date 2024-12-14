using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjBtn : MonoBehaviour
{
    public enum BtnType
    {
        Spr = 0,
        Color = 1,
        None = 2
    }

    public BtnType btnType = BtnType.Spr;

    private SpriteRenderer btnImg;
    public Sprite oriSpr;
    public Sprite overSpr;
    public Sprite clickSpr;

    public Color oriColor;
    public Color overColor;
    public Color clickColor;

    bool isOver = false;
    public GameObject activeExceptionObj; //정해논 오브젝트가 활성화되어있으면 마우스 이펙트 발동안하게

    public SpriteRenderer content;

    private void Awake()
    {
        btnImg = this.GetComponent<SpriteRenderer>();
        isOver = false;
    }

    private void OnEnable()
    {
        InitBtn();
    }

    public void SetName(string pName)
    {
        this.name = pName;
    }
    public void SetContent(Sprite pSpr)
    {
        content.sprite = pSpr;
    }

    public bool CheckActiveObj()
    {
        if (activeExceptionObj.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMouseOver()
    {
        if (CheckActiveObj()) return;
        if (isOver) return;
        isOver = true;

        SoundManager.instance.PlaySFXByKey("MouseOver");

        if (btnType == BtnType.None) return;

        if (btnType == BtnType.Spr)
        {
            if (overSpr != null)
            {
                btnImg.sprite = overSpr;
            }
        }
        else
        {
            btnImg.color = overColor;
        }
    }
    private void OnMouseExit()
    {
        if (!isOver) return;
        isOver = false;

        InitBtn();

    }

    private void OnMouseUp()
    {
        if (CheckActiveObj()) return;

        SoundManager.instance.PlaySFXByKey("MouseClick");
        if (btnType == BtnType.None) return;

        if (btnType == BtnType.Spr)
        {
            if (clickSpr != null)
            {
                btnImg.sprite = clickSpr;
            }
        }
        else
        {
            btnImg.color = clickColor;
        }
    }

    public void InitBtn()
    {
        if (btnType == BtnType.None) return;

        if (btnType == BtnType.Spr)
        {
            if (oriSpr != null)
            {
                btnImg.sprite = oriSpr;
            }
        }
        else
        {
            btnImg.color = oriColor;
        }
    }
}
