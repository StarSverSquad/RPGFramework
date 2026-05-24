using System;
using RPGF.EventSystem;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "EntityState", menuName = "RPG/EntityState")]
    public class RPGEntityState : ScriptableObject
    {
        public GlobalEvent Event;

        public string Tag;
        public string Name;

        public Color Color;

        public Sprite Icon;

        [Tooltip("Пропускать ход")]
        public bool SkipTurn;

        public bool OnlyForBattle;

        public int AddHeal;
        public int AddMana;

        public int AddDamage;
        public int AddDefense;
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
}