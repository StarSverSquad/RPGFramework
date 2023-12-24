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

    [Tooltip("���� ������, �� ������� ��� ������")]
    public List<string> RequireClasses = new List<string>();

    [Header("��������")]
    public int Heal;
    public int Mana;

    public int Damage;
    public int Defence;
    public int Agility;
    public int Luck;
}
