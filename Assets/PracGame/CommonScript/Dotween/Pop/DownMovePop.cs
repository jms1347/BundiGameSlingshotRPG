using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DownMovePop : PopParent
{
    public override void OpenPop()
    {
        this.transform.DOKill();
        this.transform.DOLocalMoveY(0, 0.8f).SetEase(Ease.Flash);
    }

    public override void ClosePop()
    {
        this.transform.DOKill();
        this.transform.DOLocalMoveY(oriPos.y, 0.8f).SetEase(Ease.Flash);
    }
}
