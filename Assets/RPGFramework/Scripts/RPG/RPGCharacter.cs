using RPGF.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "RPG/Character")]
public class RPGCharacter : RPGEntity
{
    [Header("Настройки персонажа")]
    public string Class;

    public Sprite CommonImage;
    public Sprite BattleImage;
    public Sprite BattleSmallImage;

    public Color Color;

    public PlayableCharacterModelController Model;

    public bool ParticipateInBattle = true;
    public bool CanMoveInBattle = true;

    [Header("Уровень")]
    public int Level = 1;
    public int Expireance = 0;
    public int ExpireanceBorder = 0;
    public AnimationCurve ExpireanceBorderFactor;

    [Header("Кривые развития")]
    public AnimationCurve LvlHealFactor;
    public AnimationCurve LvlManaFactor;
    public AnimationCurve LvlDamageFactor;
    public AnimationCurve LvlDefenceFactor;
    public AnimationCurve LvlAgilityFactor;
    public AnimationCurve LvlLuckFactor;

    [Header("Способности")]
    public List<RPGAbility> Abilities = new List<RPGAbility>();
   
    public const RPGWerable.UsedType WeaponType = RPGWerable.UsedType.Weapon;
    [Header("Вещи")]
    public RPGWeapon WeaponSlot = null;

    public const RPGWerable.UsedType HeadType = RPGWerable.UsedType.Head;
    public RPGWerable HeadSlot = null;

    public const RPGWerable.UsedType BodyType = RPGWerable.UsedType.Body;
    public RPGWerable BodySlot = null;

    public const RPGWerable.UsedType ShieldType = RPGWerable.UsedType.Shield;
    public RPGWerable ShieldSlot = null;

    public const RPGWerable.UsedType TalismanType = RPGWerable.UsedType.Talisman;
    public RPGWerable TalismanSlot = null;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
    }

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
        Level++;

        Expireance = 0;

        ExpireanceBorder = (int)(ExpireanceBorder * ExpireanceBorderFactor.Evaluate(Level));

        DefaultHeal = (int)(LvlHealFactor.Evaluate(Level) * DefaultHeal);
        DefaultMana = (int)(LvlManaFactor.Evaluate(Level) * DefaultMana);
        DefaultDamage = (int)(LvlDamageFactor.Evaluate(Level) * DefaultDamage);
        DefaultDefence = (int)(LvlDefenceFactor.Evaluate(Level) * DefaultDefence);
        DefaultAgility = (int)(LvlAgilityFactor.Evaluate(Level) * DefaultAgility);
        DefaultLuck = (int)(LvlLuckFactor.Evaluate(Level) * DefaultAgility);

        UpdateStats();

        Heal = MaxHeal; Mana = MaxMana;
    }
}
