using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineBar : MonoBehaviour
{
    [SerializeField]
    private Image Bar;
    [SerializeField]
    private RectTransform refRect;

    public bool SwapAxis = false;

    public void SetValue(float value)
    {
        if (!SwapAxis)
            Bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * refRect.sizeDelta.x);
        else
            Bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value * refRect.sizeDelta.y);
    }

    public void SetValue(float value, float maxValue)
    {
        SetValue(value / maxValue);
    }
}
