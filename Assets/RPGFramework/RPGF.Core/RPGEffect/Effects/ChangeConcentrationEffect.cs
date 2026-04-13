using RPGF.Battle;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    public class ChangeConcentrationEffect : RPGEffectBase
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
}