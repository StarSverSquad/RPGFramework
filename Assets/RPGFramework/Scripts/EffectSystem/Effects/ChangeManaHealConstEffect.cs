using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealConstEffect : EffectBase
{
    public int Heal;
    public int Mana;

    public override IEnumerator BattleInvoke(BattleEntityInfo user, BattleEntityInfo target)
    {
        AddInfo("Heal", Heal);
        AddInfo("Mana", Mana);

        target.Heal += Heal;
        target.Mana += Mana;

        yield break;
    }

    public override IEnumerator ExplorerInvoke(RPGEntity user, RPGEntity target)
    {
        target.Heal += Heal;
        target.Mana += Mana;

        yield break;
    }

    public override string GetName()
    {
        return "Изменить HP\\MP";
    }
}