using System;
using System.Collections;
using UnityEngine;

public class InvokeEventEffect : EffectBase
{
    public GraphEvent @event;

    public override string GetName()
    {
        return "Запустить событие";
    }
}