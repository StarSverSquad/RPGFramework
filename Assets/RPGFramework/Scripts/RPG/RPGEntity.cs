using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGEntity : ScriptableObject
{
    public string Name;

    [Multiline(3)]
    public string Description;

    [Header("Entity options")]
    public int DefaultHeal;
    public int DefaultMana;

    public int DefaultDamage;
    public int DefaultDefence;
    public int DefaultAgility;

    private int heal;
    public int Heal
    {
        get => heal; set => heal = Mathf.Clamp(value, 0, MaxHeal);
    }

    private int mana;
    public int Mana
    {
        get => mana; set => mana = Mathf.Clamp(value, 0, MaxMana);
    }

    public int MaxHeal { get; set; }
    public int MaxMana { get; set; }

    public int Damage { get; set; }
    public int Agility { get; set; }
    public int Defence { get; set; }

    public virtual void InitializeEntity()
    {
        UpdateStats();

        Heal = MaxHeal; Mana = MaxMana;
    }

    public virtual void UpdateStats()
    {
        Damage = DefaultDamage;
        Defence = DefaultDefence;
        Agility = DefaultAgility;

        MaxHeal = DefaultHeal;
        MaxMana = DefaultMana;
    }

    /// <summary>
    /// Наносит урон данному entity
    /// </summary>
    /// <param name="dontHurt">Если нужно просто расчитать урон</param>
    /// <returns>Потраченое хп</returns>
    public virtual int GiveDamage(int damage, bool dontHurt = false)
    {
        int dgmdefdif = damage - Mathf.RoundToInt(Defence / .5f);

        if (damage <= 0)
            return 0;

        if (!dontHurt)
            Heal -= dgmdefdif;

        return dgmdefdif;
    }
}
