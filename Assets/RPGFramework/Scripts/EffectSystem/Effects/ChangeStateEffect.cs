using System;
using System.Collections;
using UnityEngine;

public class ChangeStateEffect : EffectBase
{
    public bool IsAddState = true;

    public RPGEntityState State;

    public override IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        if (target is BattleCharacterInfo characterInfo && characterInfo.IsDead)
            yield break;

        if (IsAddState)
        {
            AddInfo("AddState", State);
            target.AddState(State);
        }
        else
        {
            AddInfo("RemoveState", State);
            target.RemoveState(State);
        }

        yield break;
    }

    public override string GetName()
    {
        return "Изменить состояние";
    }
}