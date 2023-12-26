using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NumeriticBar : MonoBehaviour
{
    [SerializeField]
    private LineBar lineBar;

    [SerializeField]
    private TextMeshProUGUI barText;

    [SerializeField]
    private int maxValue;
    public int MaxValue => maxValue;

    [SerializeField]
    private int value;
    public int Value => value;

    public void SetValue(int value, int maxValue, string separator = " / ")
    {
        lineBar.SetValue((float)value / (float)maxValue);

        barText.text = $"{value}{separator}{maxValue}";

        this.value = value;
        this.maxValue = maxValue;
    }
}
