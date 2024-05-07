using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInfo", menuName = "RPG/BattleInfo")]
public class RPGBattleInfo : ScriptableObject
{
    [Header("����� ���������")]
    public RPGEnemySquad enemySquad;

    [Tooltip("���� �������� ������?")]
    public bool EnemyStart = false;

    [Tooltip("����� �� ���������?")]
    public bool CanLose = true;

    [Tooltip("����� �� �������?")]
    public bool CanFlee = false;

    public bool ShowStartMessage = true;
    public bool ShowEndMessage = true;

    [Header("��������� �����")]
    public AudioClip BattleMusic;
    public float MusicVolume;

    [Tooltip("��������� �� ���������� ������?")]
    public bool StopGlobalMusic = true;

    [Header("���������")]
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
