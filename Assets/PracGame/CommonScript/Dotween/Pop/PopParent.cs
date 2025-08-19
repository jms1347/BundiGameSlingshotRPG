using System;
using UnityEngine;
using UnityEngine.UI;

public class PopParent : MonoBehaviour
{
    public Vector3 oriPos;
    public Button popBtn;

    private Action callbackClosePopAction;

    public Action CallbackClosePopAction { get => callbackClosePopAction; set => callbackClosePopAction = value; }

    public virtual void Awake()
    {
        oriPos = this.transform.localPosition;

        if(popBtn == null)
        {
            popBtn = this.transform.GetChild(1).GetComponent<Button>();
        }
    }
    public virtual void Start()
    {
        popBtn.onClick.RemoveListener(ClosePop);
        popBtn.onClick.AddListener(ClosePop);
    }

    public virtual void OpenPop() { }
    public virtual void ClosePop() { }

}
