using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealConstEffect : EffectBase
{
    public int Heal;
    public int Mana;

    public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
    {
        target.Heal += Mathf.RoundToInt(Heal * Factor);
        target.Mana += Mathf.RoundToInt(Mana * Factor);

        yield break;
    }

    public override string GetName()
    {
        return "Изменить HP\\MP";
    }
}