﻿using System;
using System.Collections;
using UnityEngine;

public class RemoveAllStatesEffect : EffectBase
{
    public override IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        target.Entity.RemoveAllStates();

        yield break;
    }

    public override string GetName()
    {
        return "Убрать все состояния";
    }
}