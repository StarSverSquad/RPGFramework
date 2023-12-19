using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "RPG/Character")]
public class RPGCharacter : RPGEntity
{
    [Header("Character options")]
    public string Class;

    public Sprite Icon;

    public DynamicExplorerObject Model;

    public bool ParticipateInBattle = true;
    public bool CanMoveInBattle = true;

    [Header("Level")]
    public int Level = 1;
    public int Expireance = 0;
    public int ExpireanceBorder = 0;

    public float ExpireanceBorderFactor = 1.25f;

    [Header("LevelUp factors")]
    public float LvlHealFactor = 1f;
    public float LvlManaFactor = 1f;
    public float LvlDamageFactor = 1f;
    public float LvlDefenceFactor = 1f;
    public float LvlAgilityFactor = 1f;

    [Header("Abilitys")]
    public List<RPGAbility> Abilities = new List<RPGAbility>();
   
    public const RPGWerable.UsedType WeaponType = RPGWerable.UsedType.Weapon;
    [Header("Equipment")]
    public RPGWeapon WeaponSlot = null;

    public const RPGWerable.UsedType HeadType = RPGWerable.UsedType.Head;
    public RPGWerable HeadSlot = null;

    public const RPGWerable.UsedType BodyType = RPGWerable.UsedType.Body;
    public RPGWerable BodySlot = null;

    public const RPGWerable.UsedType ShieldType = RPGWerable.UsedType.Shield;
    public RPGWerable ShieldSlot = null;

    public const RPGWerable.UsedType TalismanType = RPGWerable.UsedType.Talisman;
    public RPGWerable TalismanSlot = null;

    public override void UpdateStats()
    {
        base.UpdateStats();

        if (WeaponSlot != null)
        {
            MaxHeal += WeaponSlot.Heal;
            MaxMana += WeaponSlot.Mana;

            Damage += WeaponSlot.Damage;
            Defence += WeaponSlot.Defence;
            Agility += WeaponSlot.Agility;
        }

        if (HeadSlot != null )
        {
            MaxHeal += HeadSlot.Heal;
            MaxMana += HeadSlot.Mana;

            Damage += HeadSlot.Damage;
            Defence += HeadSlot.Defence;
            Agility += HeadSlot.Agility;
        }

        if (BodySlot != null)
        {
            MaxHeal += BodySlot.Heal;
            MaxMana += BodySlot.Mana;

            Damage += BodySlot.Damage;
            Defence += BodySlot.Defence;
            Agility += BodySlot.Agility;
        }

        if (ShieldSlot != null)
        {
            MaxHeal += ShieldSlot.Heal;
            MaxMana += ShieldSlot.Mana;

            Damage += ShieldSlot.Damage;
            Defence += ShieldSlot.Defence;
            Agility += ShieldSlot.Agility;
        }

        if (TalismanSlot != null)
        {
            MaxHeal += TalismanSlot.Heal;
            MaxMana += TalismanSlot.Mana;

            Damage += TalismanSlot.Damage;
            Defence += TalismanSlot.Defence;
            Agility += TalismanSlot.Agility;
        }

        Heal = Math.Clamp(Heal, 0, MaxHeal);
        Mana = Math.Clamp(Mana, 0, MaxMana);
    }

    public virtual bool LevelUpCanExecute()
    {
        return Expireance >= ExpireanceBorder;
    }

    public void LevelUp()
    {
        Expireance -= ExpireanceBorder;

        ExpireanceBorder = (int)(ExpireanceBorder * ExpireanceBorderFactor);

        Level++;

        DefaultHeal = (int)(LvlHealFactor * DefaultHeal);
        DefaultMana = (int)(LvlManaFactor * DefaultMana);
        DefaultDamage = (int)(LvlDamageFactor * DefaultDamage);
        DefaultDefence = (int)(LvlDefenceFactor * DefaultDefence);
        DefaultAgility = (int)(LvlAgilityFactor * DefaultAgility);
    }
}
