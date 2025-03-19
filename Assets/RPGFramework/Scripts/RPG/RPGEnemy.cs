using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "RPG/Enemy")]
public class RPGEnemy : RPGEntity
{
    [Serializable]
    public struct EnemyAct
    {
        public string Name;
        public string Description;
        public GraphEvent Event;

        public bool OnlyOne;

        public static EnemyAct NullAct => new EnemyAct() { Name = "NULL" };
    }

    [Tooltip("—юда нужны объекты которые имеют RPGAttackPattern!")]
    public List<BattleAttackPatternBase> Patterns = new List<BattleAttackPatternBase>();

    public List<EnemyAct> Acts = new List<EnemyAct>();

    public GameObject EnemyModel;
}


