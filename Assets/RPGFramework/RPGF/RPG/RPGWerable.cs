using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{

    [CreateAssetMenu(fileName = "WerableItem", menuName = "RPG/WerableItem")]
    public class RPGWerable : RPGCollectable
    {
        public enum UsedType
        {
            Head, Body, Shield, Accessory, Weapon
        }

        public UsedType UsedOn;

        [Tooltip("Требуется класс, чтобы использовать этот предмет")]
        public List<string> RequireClasses = new();

        [Header("Статистика предмета:")]
        public int Heal;
        public int Mana;

        public int Damage;
        public int Defense;
        public int Agility;
        public int Luck;
    }
}
