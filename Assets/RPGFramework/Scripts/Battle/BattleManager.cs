using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : ContentManagerBase, IManagerInitialize
{
    public static BattleManager Instance;

    public BattlePipeline pipeline;
    public CharacterBoxManager characterBox;
    public BattleChoiceManager choice;
    public BattleUtility utility;
    public BattleData data;
    public BattleFieldManager battleField;
    public BattleCharacterPreview characterPreview;
    public BattleBackground background;
    public BattleAudioManager battleAudio;
    public BattlePlayerManager player;
    public AttackQTEManager attackQTE;
    public EnemyModelsManager enemyModels;
    public ConcentrationBarManager concentrationBar;
    public BattleDescription description;
    public BattlePatternManager pattern;
    public BattleVisualTransmitionManager visualTransmition;
    public BattleUIShake Shaker;

    public static BattleUtility Utility => Instance.utility;
    public static BattleData Data => Instance.data;

    public static bool IsBattle => Instance.pipeline.MainIsWorking;

    public static void StartBattle(RPGBattleInfo info) => Utility.StartBattle(info); 

    public void Initialize()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        player.SetActive(false);
    }
}
