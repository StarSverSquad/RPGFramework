﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShowImageAction : GraphActionBase
{
    public Sprite ImageSprite;
    public float FadeTime;

    public bool IsWait;

    private MediaManager Media => CommonManager.Instance.Media;

    public ShowImageAction() : base("ShowImage")
    {
        ImageSprite = null;
        FadeTime = 0;

        IsWait = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        Media.ShowImage(ImageSprite, FadeTime);

        if (IsWait)
            yield return new WaitWhile(() => Media.IsFade);
    }

    public override string GetHeader()
    {
        return "Отобразить цвет";
    }
}