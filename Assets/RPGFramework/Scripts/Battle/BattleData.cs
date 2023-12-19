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
    public List<BattleEnemyInfo> Enemys = new List<BattleEnemyInfo>();
    [HideInInspector]
    public List<BattleCharacterInfo> Characters = new List<BattleCharacterInfo>();

    [Header("Ссылки")]
    public AttackEffect DefaultEffect;

    public DamageText DmgText;

    public Canvas BattleCanvas;

    public int Concentration { get; set; } = 0;

    [Header("Настройки")]
    public int MaxConcentration;

    public int AdditionConcentrationOnDefence = 10;

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
        
    }
}