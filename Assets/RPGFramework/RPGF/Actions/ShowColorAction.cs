using RPGF.EventSystem;
using RPGF.Shared;
using System.Collections;
using UnityEngine;

public class ShowColorAction : GraphActionBase
{
    public Color Color;
    public float FadeTime;

    public bool IsWait;

    private MediaManager Media => SharedManager.Instance.Media;

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
