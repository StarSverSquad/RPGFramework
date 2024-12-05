using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QTEEffectLine : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    public Color lineColor
    {
        set
        {
            image.color = value;
        }
        get => image.color;
    }

    public void Invoke()
    {
        rectTransform.DOSizeDelta(new Vector2(0, 100), 1f).SetRelative().SetEase(Ease.OutExpo).Play();

        Color newColor = lineColor;
        newColor.a = 0;

        image.DOColor(newColor, 1f).SetEase(Ease.OutExpo).Play();

        Destroy(gameObject, 1.1f);
    }
}