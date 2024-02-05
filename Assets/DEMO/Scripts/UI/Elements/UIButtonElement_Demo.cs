using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
class UIButtonElement_Demo : UIElementBase
{
    [SerializeField]
    private Image buttonImage;
    [Space]
    [SerializeField]
    private Color startColor = Color.white;
    [SerializeField]
    private Color endColor = Color.white;
    [SerializeField]
    private float fadeTime = 1.0f;

    private bool submit = false;

    private void OnEnable()
    {
        buttonImage.color = startColor;
    }

    private void OnValidate()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
    }

    public override void OnSubmit()
    {
        submit = true;
    }

    public override void OnTransmition(UISectionBase.TransmitionDirection direction)
    {
        submit = false;
    }

    public override void OnFocus()
    {
        buttonImage.color = startColor;

        DOTween.Sequence(transform)
                        .Append(buttonImage.DOColor(endColor, fadeTime / 2f))
                        .Append(buttonImage.DOColor(startColor, fadeTime / 2f))
                        .SetLoops(-1)
                        .Play();    
    }

    public override void OnLostFocus()
    {
        transform.DOKill();
        buttonImage.color = submit ? endColor : startColor;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
