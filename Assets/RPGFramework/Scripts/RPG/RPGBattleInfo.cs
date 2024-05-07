using System;
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

    public bool ShowStartMessage = true;
    public bool ShowEndMessage = true;

    [Header("Настройки аудио")]
    public AudioClip BattleMusic;
    public float MusicVolume;

    [Tooltip("Заглушать ли глобальную музыку?")]
    public bool StopGlobalMusic = true;

    [Header("Остальное")]
    public GameObject Background;

    [Space]
    public VisualBattleTransmitionEffectBase BattleEnterEffect;
    public VisualBattleTransmitionEffectBase BattleExitEffect;

    [SerializeReference]
    [HideInInspector]
    public List<RPGBattleEvent> Events = new List<RPGBattleEvent>();
}

[Serializable]
public class RPGBattleEvent
{
    public enum InvokePeriod
    {
        NoWay, EveryPlayerTurn, EveryEnemyTurn, OnPlayerTurn, OnEnemyTurn, 
        OnWin, OnFlee, OnLose, OnBattleStart, OnBattleEnd, BeforeHit, AfterHit,
        OnLessEnemyHeal, OnLessCharacterHeal
    }

    public InvokePeriod Period;

    public bool IsCustomEvent;

    public CustomActionBase CustomAction;
    public GraphEvent Event;

    public int Turn;

    public string EntityTag;
    public float Heal;
}
