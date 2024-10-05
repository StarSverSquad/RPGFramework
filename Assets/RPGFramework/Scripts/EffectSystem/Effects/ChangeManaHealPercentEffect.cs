using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealPercentEffect : EffectBase
{
    public float Heal;
    public float Mana;

    public override IEnumerator Invoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        target.Heal += Mathf.RoundToInt(target.Entity.MaxHeal * Heal);
        target.Mana += Mathf.RoundToInt(target.Entity.MaxMana * Mana);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить HP%\\MP%";
    }
}