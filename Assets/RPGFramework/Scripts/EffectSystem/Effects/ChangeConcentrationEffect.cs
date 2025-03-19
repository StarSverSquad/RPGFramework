using System.Collections;
using UnityEngine;

public class ChangeConcentrationEffect : EffectBase
{
    public int AddConcentration;

    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        BattleManager.BattleUtility.AddConcetration(Mathf.RoundToInt(AddConcentration * Factor));

        yield break;
    }

    public override string GetName()
    {
        return "Изменить концентрацию";
    }
}