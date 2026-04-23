using RPGF.Core.RPGEffect.Attributes;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    [UseRPGEffect("Изменить HP\\MP")]
    public class ChangeManaHealConstEffect : RPGEffectBase
    {
        public int Heal;
        public int Mana;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            target.Heal += Mathf.RoundToInt(Heal * Factor);
            target.Mana += Mathf.RoundToInt(Mana * Factor);

            yield break;
        }
    }
}