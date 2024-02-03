using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterSaveInfo
{
    public string Tag;

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

    public string WeaponTag;
    public string HeadTag;
    public string BodyTag;
    public string ShieldTag;
    public string TalismanTag;

    public CharacterSaveInfo()
    {
        Abilities = new List<string>();
        States = new List<string>();
    }
}
