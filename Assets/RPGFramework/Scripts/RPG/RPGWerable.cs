using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WerableItem", menuName = "RPG/WerableItem")]
public class RPGWerable : RPGCollectable
{
    public enum UsedType
    {
        Head, Body, Shield, Talisman, Weapon
    }

    public UsedType UsedOn;

    [Tooltip("Если пустой, то подойдёт для любого")]
    public string RequiredClass;

    [Header("Свойства")]
    public int Heal;
    public int Mana;

    public int Damage;
    public int Agility;
    public int Defence;
}
