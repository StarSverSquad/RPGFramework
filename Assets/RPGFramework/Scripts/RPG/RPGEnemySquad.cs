using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public string Name;

    public List<EnemySlot> Enemies = new List<EnemySlot>();

    [Header("Дроп денег")]
    public bool MoneyConstDrop;
    public int Money;

    [Header("Дроп опыта")]
    public bool ExpConstDrop;
    public int Expireance;

    [Header("Дроп")]
    public List<SquadDrop> EnemiesDrop = new List<SquadDrop>();
}
