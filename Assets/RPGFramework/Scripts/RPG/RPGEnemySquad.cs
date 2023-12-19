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

    public string Name;

    public List<EnemySlot> Enemies = new List<EnemySlot>();
}
