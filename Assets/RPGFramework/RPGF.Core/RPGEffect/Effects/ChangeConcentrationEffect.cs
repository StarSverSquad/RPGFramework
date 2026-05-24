using RPGF.Battle;
using RPGF.Core.RPGEffect.Attributes;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    [UseRPGEffect("Изменить концентрацию")]
    public class ChangeConcentrationEffect : RPGEffectBase
    {
        public int AddConcentration;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            BattleManager.BattleUtility.AddConcetration(Mathf.RoundToInt(AddConcentration * Factor));

            yield break;
        }
    }
}