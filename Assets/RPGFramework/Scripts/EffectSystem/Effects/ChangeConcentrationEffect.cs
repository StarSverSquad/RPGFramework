using System;
using System.Collections;
using UnityEngine;

public class ChangeConcentrationEffect : EffectBase
{
    public int AddConcentration;

    public override IEnumerator Invoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        BattleManager.Utility.AddConcetration(AddConcentration);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить концентрацию";
    }
}