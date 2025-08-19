using UnityEngine;
using DG.Tweening;
using System;

public class ActivePop : PopParent
{
    public override void OpenPop()
    {
        this.gameObject.SetActive(true);
    }

    public override void ClosePop()
    {
        this.gameObject.SetActive(false);

        if(CallbackClosePopAction != null)
            CallbackClosePopAction();
    }
}
