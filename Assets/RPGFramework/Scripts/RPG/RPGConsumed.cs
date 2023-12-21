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

    public bool AddInPercents = false;

    public int AddHeal;
    public int AddMana;

    public int AddConcentration;

    public bool WakeupCharacter = false;

    public List<RPGEntityState> AddStates = new();

    public bool ForAlive = true;
    public bool ForDeath = true;

    public bool WriteMessage = true;
}
