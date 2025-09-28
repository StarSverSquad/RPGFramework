using RPGF.EventSystem;
using RPGF.Shared;
using System.Collections;
using UnityEngine;

public class CloseMediaAction : ActionBase
{
    public float FadeTime;

    public bool IsWait;

    private MediaManager Media => SharedManager.Instance.Media;

    public CloseMediaAction() : base("CloseMedia")
    {
        FadeTime = 0;

        IsWait = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        Media.HideImage(FadeTime);

        if (IsWait)
            yield return new WaitWhile(() => Media.IsFade);
    }

    public override string GetHeader()
    {
        return "Убрать медиа";
    }

    public override string GetInfo()
    {
        return @"Убирает изображение из событий 'Отобразить цвет' и 'Отобразить фото'";
    }
}
