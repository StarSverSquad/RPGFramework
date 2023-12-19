using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumedItem", menuName = "RPG/ConsumedItem")]
public class RPGConsumed : RPGCollectable
{
    public enum ConsumingDirection
    {
        AllTeam, Teammate, AllEnemys, Enemy, Any, All
    }

    public ConsumingDirection Direction;

    public AttackEffect AttackEffect;

    public int AddHeal;
    public int AddMana;
    public int AddConcentration;

    public bool CharacterBecomeAlive = false;

    public bool ForAlive = true;
    public bool ForDeath = true;

    public bool WriteMessage = true;
}
