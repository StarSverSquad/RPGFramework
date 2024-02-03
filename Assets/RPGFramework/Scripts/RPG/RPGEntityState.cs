using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityState", menuName = "RPG/EntityState")]
public class RPGEntityState : ScriptableObject
{
    public GraphEvent Event;

    public string Tag;
    public string Name;

    public Color Color;

    public Sprite Icon;

    [Tooltip("Блокирует возможность хода!")]
    public bool SkipTurn;

    public bool OnlyForBattle;

    public int AddHeal;
    public int AddMana;

    public int AddDamage;
    public int AddDefence;
    public int AddAgility;
    public int AddLuck;

    public int TurnCount;
}

[Serializable]
public class RPGEntityStateInstance
{
    public RPGEntityState Original;

    public int TurnsLeft;

    public RPGEntityStateInstance(RPGEntityState state)
    {
        Original = state;
        TurnsLeft = Original.TurnCount;
    }
}