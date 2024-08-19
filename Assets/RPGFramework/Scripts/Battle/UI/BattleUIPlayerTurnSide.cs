using DG.Tweening;
using UnityEngine;

public class BattleUIPlayerTurnSide : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    public float TraslationTime => 1f;

    public void Show()
    {
        rect.DOAnchorPosY(165, TraslationTime).SetLoops(0).SetEase(Ease.OutSine).Play();
    }

    public void Hide()
    {
        rect.DOAnchorPosY(-521, TraslationTime).SetLoops(0).SetEase(Ease.InSine).Play();             
    }
}