using RPGF.Core.RPGEffect.Attributes;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    [UseRPGEffect("Изменить HP%\\MP%")]
    public class ChangeManaHealPercentEffect : RPGEffectBase
    {
        public float Heal;
        public float Mana;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            target.Heal += Mathf.RoundToInt(target.MaxHeal * Heal * Factor);
            target.Mana += Mathf.RoundToInt(target.MaxMana * Mana * Factor);

            yield break;
        }
    }
}