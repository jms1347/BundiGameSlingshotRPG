using UnityEngine;
using DG.Tweening;

public class DimdPop : PopParent
{
    public GameObject dimd;

    public override void Awake()
    {
        base.Awake();
        if(dimd == null)
        {
            dimd = this.transform.GetChild(0).gameObject;
        }
    }
    public override void OpenPop()
    {
        PopupManager.Instance.OpenAni(this.gameObject, dimd);
    }

    public override void ClosePop()
    {
        PopupManager.Instance.CloseAni(this.gameObject, dimd);

        if(CallbackClosePopAction != null)
            CallbackClosePopAction();
    }
}
