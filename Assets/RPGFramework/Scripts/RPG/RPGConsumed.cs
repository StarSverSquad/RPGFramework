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

    public VisualAttackEffect VisualEffect;

    public bool WakeupCharacter = false;

    public bool ForAlive = true;
    public bool ForDeath = true;

    public bool WriteMessage = true;

    [HideInInspector]
    [SerializeReference]
    public List<EffectBase> Effects = new List<EffectBase>();
}
