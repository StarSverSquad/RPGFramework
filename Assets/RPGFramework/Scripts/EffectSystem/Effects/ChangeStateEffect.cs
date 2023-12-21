using System;
using System.Collections;
using UnityEngine;

public class ChangeStateEffect : EffectBase
{
    public bool IsAddState = true;

    public RPGEntityState State;

    public override string GetName()
    {
        return "Изменить состояние";
    }
}