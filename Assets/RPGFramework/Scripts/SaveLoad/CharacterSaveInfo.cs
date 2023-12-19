using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterSaveInfo
{
    public string Name;

    public int Heal;
    public int Mana;

    public int DefaultHeal;
    public int DefaultMana;

    public int DefaultDamage;
    public int DefaultDefance;
    public int DefaultAgility;

    public int Level;
    public int Expireance;
    public int ExpireanceBorder;

    public List<string> Abilitys;

    public string WeaponName;
    public string HeadName;
    public string BodyName;
    public string GlovesName;
    public string BoodsName;
    public string TalismanName;

    public CharacterSaveInfo()
    {
        Abilitys = new List<string>();
    }
}
