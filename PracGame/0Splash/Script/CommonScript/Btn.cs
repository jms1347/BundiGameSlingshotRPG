using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum BtnType
    {
        Spr = 0,
        Color = 1,
        None = 2
    }

    public BtnType btnType = BtnType.Spr;

    private Image btnImg;
    public Sprite oriSpr;
    public Sprite overSpr;
    public Sprite clickSpr;

    public Color oriColor;
    public Color overColor;
    public Color clickColor;

    private void Awake()
    {
        btnImg = this.GetComponent<Image>();
    }

    private void OnEnable()
    {
        InitBtn();

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySoundByKey(SoundManager.SoundType.UI, "MouseOver");

        if (btnType == BtnType.None) return;

        if (btnType == BtnType.Spr)
        {
            if (overSpr != null)
            {
                btnImg.sprite = overSpr;
                btnImg.SetNativeSize();
            }
        }
        else
        {
            btnImg.color = overColor;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InitBtn();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlaySoundByKey(SoundManager.SoundType.UI, "MouseClick");
        if (btnType == BtnType.None) return;

        if (btnType == BtnType.Spr)
        {
            if (clickSpr != null)
            {
                btnImg.sprite = clickSpr;
                btnImg.SetNativeSize();
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
                btnImg.SetNativeSize();
            }
        }
        else
        {
            btnImg.color = oriColor;
        }
    }
}
