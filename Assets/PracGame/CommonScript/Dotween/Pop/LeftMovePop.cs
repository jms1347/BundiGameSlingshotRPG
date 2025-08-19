using UnityEngine;
using DG.Tweening;
public class LeftMovePop : PopParent
{
    public override void OpenPop()
    {
        this.transform.DOKill();
        this.transform.DOLocalMoveX(0, 0.8f).SetEase(Ease.Flash);
    }

    public override void ClosePop()
    {
        this.transform.DOKill();
        this.transform.DOLocalMoveX(oriPos.x, 0.8f).SetEase(Ease.Flash);
    }
}
