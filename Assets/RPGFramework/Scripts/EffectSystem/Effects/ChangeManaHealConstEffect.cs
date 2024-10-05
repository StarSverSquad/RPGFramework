using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealConstEffect : EffectBase
{
    public int Heal;
    public int Mana;

    public override IEnumerator Invoke(BattleEntityInfo user, BattleEntityInfo target)
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