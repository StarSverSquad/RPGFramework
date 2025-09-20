using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceItem : MonoBehaviour, IManagerInitialize
{
    [SerializeField]
    private TextMeshProUGUI TextField;

    public string Title
    {
        get { return TextField.text; }
        set { TextField.text = value; }
    }

    private float xSize = 0;
    public float XSize => xSize;

    public void Initialize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        xSize = TextField.GetPreferredValues().x;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, XSize);
    }
}
