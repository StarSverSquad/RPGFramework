using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

[Serializable]
public class BattleEntityInfo
{
    public RPGEntity Entity { get; set; }

    public int Heal
    {
        get => Entity.Heal;
        set => Entity.Heal = value;
    }
    public int Mana
    {
        get => Entity.Mana;
        set => Entity.Mana = value;
    }

    public RPGEntityState[] States => Entity.States;

    public BattleEntityInfo(RPGEntity entity)
    {
        Entity = entity;
    }
}
