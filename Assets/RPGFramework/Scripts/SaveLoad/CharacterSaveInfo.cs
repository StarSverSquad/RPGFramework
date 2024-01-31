using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

[Serializable]
public class CharacterSaveInfo
{
    public string Name;

    public int Heal;
    public int Mana;
    public int Level;
    public int Expirience;
    public int ExpirienceBorder;

    public bool InParty;
    public int PositionInParty;

    public int DefaultHeal;
    public int DefaultMana;
    public int DefaultDamage;
    public int DefaultDefence;
    public int DefaultAgility;

    public List<string> Abilities;
    public List<string> States;

    public string WeaponName;
    public string HeadName;
    public string BodyName;
    public string ShieldName;
    public string TalismanName;

    public CharacterSaveInfo()
    {
        Abilities = new List<string>();
        States = new List<string>();
    }
}
