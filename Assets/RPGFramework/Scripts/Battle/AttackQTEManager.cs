using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AttackQTEManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform qteContainer; 

    [SerializeField]
    private AttackQTE qte;
    public AttackQTE QTE => qte;

    [SerializeField]
    private Vector2 hidenRectPosition;
    [SerializeField]
    private Vector2 showedRectPosition;

    public float TransmitionTime => 1f;
        
    public void Invoke()
    {
        qte.Invoke();
    }

    public void Show()
    {
        qteContainer.DOAnchorPos(showedRectPosition, TransmitionTime)
            .From(hidenRectPosition).SetEase(Ease.OutSine).SetLoops(0).Play();
    }

    public void Hide()
    {
        qteContainer.DOAnchorPos(hidenRectPosition, TransmitionTime)
        .From(showedRectPosition).SetEase(Ease.InSine).SetLoops(0).Play();
    }
}