using DG.Tweening;
using UnityEngine;

public class BattleUIPlayerTurnSide : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    public float TraslationTime => 0.7f;

    public void Show()
    {
        rect.DOAnchorPosY(165, TraslationTime).SetEase(Ease.OutExpo).Play();
    }

    public void Hide()
    {
        rect.DOAnchorPosY(-521, TraslationTime).SetEase(Ease.OutExpo).Play();             
    }
}