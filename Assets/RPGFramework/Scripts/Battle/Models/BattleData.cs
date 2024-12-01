using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Текущая битва
    /// </summary>
    public RPGBattleInfo BattleInfo { get; set; } = null;

    [HideInInspector]
    public List<RPGEnemy> Enemys = new List<RPGEnemy>();
    [HideInInspector]
    public List<BattleTurnData> TurnsData = new List<BattleTurnData>();

    [Header("Ссылки")]
    public VisualAttackEffect DefaultEffect;

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

    public void Dispose()
    {
        Enemys.Clear();
        TurnsData.Clear();
        BattleInfo = null;

        Concentration = 0;
    }
}