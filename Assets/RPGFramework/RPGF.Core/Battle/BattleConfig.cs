using RPGF.Core.Battle.BattleField.Abstractions;
using RPGF.Core.Battle.Enums;
using RPGF.Domain.DI;
using UnityEngine;

namespace RPGF.Core.Battle
{
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "RPGFramework/BattleConfig", order = 1)]
    public class BattleConfig : ScriptableObject, Injectable
    {
        [Tooltip("Attack, Act, Item, Flee, Ability, Defence")]
        [SerializeField]
        private Sprite[] actionIcons = new Sprite[6];

        [Header("Префабы")]
        public BattleAttackEffect DefaultEffect;
        public FallingText DmgText;
        public BattleFieldBase DefaultBattleField;

        [Header("Настройки")]
        public int MaxConcentration;

        public int AdditionConcentrationOnDefence = 10;

        public string GameOverSceneName = "GameOver";

        [Header("Звуки")]
        public AudioClip HoverSound;
        public AudioClip SellectSound;
        public AudioClip CancelSound;
        public AudioClip DenySound;
        public AudioClip HealSound;
        public AudioClip HurtSound;
        public AudioClip EnemyDamageSound;
        public AudioClip EnemyDeathSound;
        public AudioClip MissSound;
        public AudioClip FleeSound;
        [Header("Треки")]
        public AudioClip LoseTrack;
        public AudioClip WinTrack;

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
    }
}