using System;
using System.Collections;
using UnityEngine;

public class ChangeManaHealPercentEffect : EffectBase
{
    public float Heal;
    public float Mana;

    public override string GetName()
    {
        return "Изменить HP%\\MP%";
    }
}