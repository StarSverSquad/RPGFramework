using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceItem : MonoBehaviour
{
    public TextMeshProUGUI TextField;

    public float XSize = 0;

    public void Init(string text)
    {
        TextField.text = text;

        RectTransform rectTransform = GetComponent<RectTransform>();

        XSize = TextField.GetPreferredValues().x;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, XSize);
    }
}
