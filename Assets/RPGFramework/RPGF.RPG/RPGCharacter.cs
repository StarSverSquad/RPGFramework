using System;
using System.Collections.Generic;
using RPGF.Core.Character;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "Character", menuName = "RPG/Character")]
    public class RPGCharacter : RPGEntity
    {
        [Header("Настройки персонажа")]
        public string Class = string.Empty;

        public Sprite Icon;
        public Sprite TitleImage;
        public Sprite BattleTinyIcon;
        public Sprite BattleImage;
        public Sprite BattleQueryImage;

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

        public RPGWerable HeadSlot = null;
        public RPGWerable BodySlot = null;
        public RPGWerable ShieldSlot = null;
        public RPGWerable AccessorySlot = null;

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
                Defense += WeaponSlot.Defense;
                Agility += WeaponSlot.Agility;
                Luck += WeaponSlot.Luck;
            }

            if (HeadSlot != null)
            {
                MaxHeal += HeadSlot.Heal;
                MaxMana += HeadSlot.Mana;

                Damage += HeadSlot.Damage;
                Defense += HeadSlot.Defense;
                Agility += HeadSlot.Agility;
                Luck += HeadSlot.Luck;
            }

            if (BodySlot != null)
            {
                MaxHeal += BodySlot.Heal;
                MaxMana += BodySlot.Mana;

                Damage += BodySlot.Damage;
                Defense += BodySlot.Defense;
                Agility += BodySlot.Agility;
                Luck += BodySlot.Luck;
            }

            if (ShieldSlot != null)
            {
                MaxHeal += ShieldSlot.Heal;
                MaxMana += ShieldSlot.Mana;

                Damage += ShieldSlot.Damage;
                Defense += ShieldSlot.Defense;
                Agility += ShieldSlot.Agility;
                Luck += ShieldSlot.Luck;
            }

            if (AccessorySlot != null)
            {
                MaxHeal += AccessorySlot.Heal;
                MaxMana += AccessorySlot.Mana;

                Damage += AccessorySlot.Damage;
                Defense += AccessorySlot.Defense;
                Agility += AccessorySlot.Agility;
                Luck += AccessorySlot.Luck;
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
            DefaultDefense = (int)(LvlDefenceFactor.Evaluate(Level) * DefaultDefense);
            DefaultAgility = (int)(LvlAgilityFactor.Evaluate(Level) * DefaultAgility);
            DefaultLuck = (int)(LvlLuckFactor.Evaluate(Level) * DefaultAgility);

            UpdateStats();

            Heal = MaxHeal; Mana = MaxMana;
        }

        public RPGWerable GetWerableByType(RPGWerable.UsedType type)
        {
            switch (type)
            {
                case RPGWerable.UsedType.Weapon:
                    return WeaponSlot;
                case RPGWerable.UsedType.Head:
                    return HeadSlot;
                case RPGWerable.UsedType.Body:
                    return BodySlot;
                case RPGWerable.UsedType.Shield:
                    return ShieldSlot;
                case RPGWerable.UsedType.Accessory:
                    return AccessorySlot;
                default:
                    Debug.LogError($"Unknown used type: {type}");
                    return null;
            }
        }

        public void SetWerableByType(RPGWerable.UsedType type, RPGWerable value)
        {
            if (type == RPGWerable.UsedType.Weapon && value is not RPGWeapon)
            {
                Debug.LogError($"Value is not a weapon: {value}");
                return;
            }

            switch (type)
            {
                case RPGWerable.UsedType.Weapon:
                    WeaponSlot = value as RPGWeapon;
                    break;
                case RPGWerable.UsedType.Head:
                    HeadSlot = value;
                    break;
                case RPGWerable.UsedType.Body:
                    BodySlot = value;
                    break;
                case RPGWerable.UsedType.Shield:
                    ShieldSlot = value;
                    break;
                case RPGWerable.UsedType.Accessory:
                    AccessorySlot = value;
                    break;
                default:
                    Debug.LogError($"Unknown used type: {type}");
                    break;
            }
        }
    }
}
