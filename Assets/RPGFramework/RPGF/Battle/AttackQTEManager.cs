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

    public bool IsShowed { get; private set; } = false;

    public float TransmitionTime => 0.75f;
        
    public void Invoke()
    {
        qte.Invoke();
    }

    public void Show()
    {
        qteContainer.DOAnchorPos(showedRectPosition, TransmitionTime).SetEase(Ease.OutCirc).Play().onComplete = () =>
        {
            IsShowed = true;
        };
    }

    public void Hide()
    {
        qteContainer.DOAnchorPos(hidenRectPosition, TransmitionTime).SetEase(Ease.InCirc).Play().onComplete = () =>
        {
            IsShowed = false;
        };
    }
}