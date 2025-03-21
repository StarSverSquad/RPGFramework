using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "EnemySquad", menuName = "RPG/EnemySquad")]
    public class RPGEnemySquad : ScriptableObject
    {
        [Serializable]
        public struct EnemySlot
        {
            public Vector2 ScreenPosition;
            public RPGEnemy Enemy;
        }
        [Serializable]
        public struct SquadDrop
        {
            public RPGCollectable item;
            [Range(0f, 1f)]
            public float Chance;
            [Space]
            public int Count;
            public int CountRange;
        }

        public string Tag;
        public string Name;

        public List<EnemySlot> Enemies = new List<EnemySlot>();

        [Header("���� �����")]
        public bool MoneyConstDrop;
        public int Money;

        [Header("���� �����")]
        public bool ExpConstDrop;
        public int Expireance;

        [Header("����")]
        public List<SquadDrop> EnemiesDrop = new List<SquadDrop>();
    }
}
