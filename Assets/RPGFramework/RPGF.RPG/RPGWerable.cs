using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{

    [CreateAssetMenu(fileName = "WerableItem", menuName = "RPG/WerableItem")]
    public class RPGWerable : RPGCollectable
    {
        public enum UsedType
        {
            Head, Body, Shield, Talisman, Weapon
        }

        public UsedType UsedOn;

        [Tooltip("Если пустой, то подойдёт для любого")]
        public List<string> RequireClasses = new();

        [Header("Свойства предмета:")]
        public int Heal;
        public int Mana;

        public int Damage;
        public int Defence;
        public int Agility;
        public int Luck;
    }
}
