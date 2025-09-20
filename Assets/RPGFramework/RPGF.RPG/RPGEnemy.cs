using RPGF.Battle.Enemy;
using RPGF.Battle.EnemyBehaviour;
using RPGF.Battle.Minigames;
using RPGF.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "RPG/Enemy")]
    public class RPGEnemy : RPGEntity
    {
        [Serializable]
        public struct EnemyAct
        {
            public string Name;
            public string Description;
            public GraphEvent Event;
            public MinigameBase Minigame;

            public bool OnlyOne;

            public static EnemyAct NullAct => new() { Name = "NULL" };
        }

        public BattleEnemyModel EnemyModel;

        public List<BattleEnemyBehaviourBase> Behaviours = new();

        public List<EnemyAct> Acts = new();
    }
}
