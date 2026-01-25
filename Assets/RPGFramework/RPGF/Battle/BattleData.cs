using RPGF.Core.Battle.Enums;
using RPGF.Core;
using RPGF.RPG;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.Battle
{
    public class BattleData : RPGFrameworkBehaviour, IDisposable
    {
        public RPGBattleInfo BattleInfo { get; set; } = null;

        [HideInInspector]
        public List<RPGEnemy> Enemys = new();
        [HideInInspector]
        public List<BattleTurnData> TurnsData = new();

        [Tooltip("Attack, Act, Item, Defence, Ability, Defence")]
        [SerializeField]
        private Sprite[] actionIcons = new Sprite[6];

        [Header("Ссылки")]
        public BattleAttackEffect DefaultEffect;

        public FallingText DmgText;

        public Canvas BattleCanvas;

        public int Concentration { get; set; } = 0;

        [Header("Настройки")]
        public int MaxConcentration;

        public int AdditionConcentrationOnDefence = 10;

        public string GameOverSceneName = "GameOver";

        [Header("Звуки")]
        public AudioClip Hover;
        public AudioClip Sellect;
        public AudioClip Cancel;
        public AudioClip Deny;
        public AudioClip Heal;
        public AudioClip Hurt;
        public AudioClip EnemyDamage;
        public AudioClip EnemyDeath;
        public AudioClip Miss;
        public AudioClip Lose;
        public AudioClip Win;
        public AudioClip Flee;

        public Sprite GetActionIcon(TurnAction action)
        {
            return action switch
            {
                TurnAction.Attack => actionIcons[0],
                TurnAction.Act => actionIcons[1],
                TurnAction.Item => actionIcons[2],
                TurnAction.Flee => actionIcons[3],
                TurnAction.Ability => actionIcons[4],
                TurnAction.Defence => actionIcons[5],
                _ => null,
            };
        }

        public void Dispose()
        {
            Enemys.Clear();
            TurnsData.Clear();
            BattleInfo = null;

            Concentration = 0;
        }
    }
}