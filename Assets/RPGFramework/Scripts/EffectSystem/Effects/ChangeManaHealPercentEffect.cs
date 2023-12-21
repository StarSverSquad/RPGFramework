using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealPercentEffect : EffectBase
{
    public float Heal;
    public float Mana;

    public override IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        int actualHealDif = Mathf.RoundToInt(target.Entity.MaxHeal * Heal);
        int actualManaDif = Mathf.RoundToInt(target.Entity.MaxMana * Mana);

        AddInfo("Heal", actualHealDif);
        AddInfo("Mana", actualManaDif);

        target.Heal += actualHealDif;
        target.Mana += actualManaDif;

        yield break;
    }

    public override IEnumerator ExplorerInvoke(RPGEntity user, RPGEntity target)
    {
        target.Heal = Mathf.RoundToInt(target.Heal * Heal);
        target.Mana = Mathf.RoundToInt(target.Mana * Mana);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить HP%\\MP%";
    }
}