using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : ContentManagerBase, IManagerInitialize
{
    public static BattleManager Instance;

    public BattlePipeline Pipeline;
    public BattleChoiceManager Choice;
    public BattleFieldManager BattleField;
    public BattleBackground Background;
    public BattleAudioManager BattleAudio;
    public BattlePlayerManager Player;
    public AttackQTEManager AttackQTE;
    public EnemyModelsManager EnemyModels;
    public BattlePatternManager Pattern;
    public BattleVisualTransmitionManager VisualTransmition;
    public BattleUIShake Shaker;
    public BattleUIManager UI;
    public MinigameManager Minigame;
    public BattleData data;


    public BattleUtility Utility { get; set; }
    public static BattleUtility BattleUtility => Instance.Utility;

    // TRASH
    public static BattleData Data => Instance.data;

    public static bool IsBattle => Instance.Pipeline.MainIsWorking;

    public static void StartBattle(RPGBattleInfo info) => BattleUtility.StartBattle(info); 
    //

    public void Initialize()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        Utility = new BattleUtility(this);

        Player.SetActive(false);
    }
}
