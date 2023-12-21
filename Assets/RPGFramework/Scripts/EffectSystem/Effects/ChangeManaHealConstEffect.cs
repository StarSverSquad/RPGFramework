using System;
using System.Collections;

public class ChangeManaHealConstEffect : EffectBase
{
    public int Heal;
    public int Mana;

    public override string GetName()
    {
        return "Изменить HP\\MP";
    }
}