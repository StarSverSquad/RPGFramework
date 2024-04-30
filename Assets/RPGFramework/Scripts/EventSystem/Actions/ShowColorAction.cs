using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShowColorAction : GraphActionBase
{
    public Color Color;
    public float FadeTime;

    public bool IsWait;

    private MediaManager Media => CommonManager.Instance.Media;

    public ShowColorAction() : base("ShowColor")
    {
        Color = Color.white;
        FadeTime = 0;

        IsWait = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        Media.ShowColor(Color, FadeTime);

        if (IsWait)
            yield return new WaitWhile(() => Media.IsFade);
    }

    public override string GetHeader()
    {
        return "Отобразить цвет";
    }
}
