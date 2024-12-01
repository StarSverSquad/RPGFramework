using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealPercentEffect : EffectBase
{
    public float Heal;
    public float Mana;

    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        target.Heal += Mathf.RoundToInt(target.MaxHeal * Heal);
        target.Mana += Mathf.RoundToInt(target.MaxMana * Mana);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить HP%\\MP%";
    }
}