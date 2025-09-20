using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupSunLightAction : GraphActionBase
{
    public float Intensity;

    public Color Color;

    public SetupSunLightAction() : base("SetupSunLight")
    {
        Intensity = 1.0f;

        Color = Color.white;
    }

    public override IEnumerator ActionCoroutine()
    {
        LocalManager.Instance.Sun.SetIntensity(Intensity);
        LocalManager.Instance.Sun.SetColor(Color);

        yield break;
    }

    public override string GetHeader()
    {
        return "Настройка солнечного света";
    }
}