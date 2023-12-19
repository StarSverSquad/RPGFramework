using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : ContentManagerBase
{
    public static BattleManager instance;

    public BattlePipeline pipeline;
    public CharacterBoxManager characterBox;
    public BattleChoiceManager choice;
    public BattleUtility utility;
    public BattleData data;
    public BattleFieldManager battleField;
    public CharacterPreviewManager characterPreview;
    public BattleBackgroundManager background;
    public BattleAudioManager battleAudio;
    public BattlePlayerManager player;
    public AttackQTEManager attackQTE;
    public EnemyModelsManager enemyModels;
    public ConcentrationBarManager concentrationBar;
    public BattleDescription description;
    public BattlePatternManager pattern;

    public static BattleUtility Utility => instance.utility;
    public static BattleData Data => instance.data;

    public static bool IsBattle => instance.pipeline.MainIsWorking;

    public static void StartBattle(RPGBattleInfo info) => Utility.StartBattle(info); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            InitializeChild();
        }
    }

    public override void InitializeChild() { }
}
