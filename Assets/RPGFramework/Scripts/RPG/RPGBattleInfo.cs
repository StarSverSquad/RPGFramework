using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInfo", menuName = "RPG/BattleInfo")]
public class RPGBattleInfo : ScriptableObject
{
    [Header("Общие настройки")]
    public RPGEnemySquad enemySquad;
    [Tooltip("Враг начинает первым?")]
    public bool EnemyStart = false;
    [Tooltip("Можно ли проиграть?")]
    public bool CanLose = true;
    [Tooltip("Можно ли сбежать?")]
    public bool CanFlee = false;
    [Header("Настройки аудио")]
    public AudioClip BattleMusic;
    public float MusicVolume;
    [Header("Остальное")]
    public GameObject Background;
}
