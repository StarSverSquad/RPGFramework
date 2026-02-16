using Assets.RPGFramework.RPGF.Battle;
using RPGF.Core.Battle;
using RPGF.Battle.Enemy;
using RPGF.Battle.Player;
using RPGF.Battle.UI;
using RPGF.Core;
using RPGF.Shared;
using UnityEngine;
using RPGF.Battle.Choice;
using RPGFramework.RPGF.Battle;

namespace RPGF.Battle
{
    public class BattleManager : KernelManagerBase
    {
        public static BattleManager Instance;
        public static bool IsBattle => Instance.Pipeline.MainIsWorking;

        public BattleChoiceManager Choice;
        public BattleFieldManager BattleField;
        public BattleBackground Background;
        public BattleAudioManager BattleAudio;
        public BattlePlayerManager Player;
        public AttackQTEManager AttackQTE;
        public BattleEnemyModelsManager EnemyModels;
        public BattleEnemyBehaviourManager EnemyBehaviour;
        public BattleVisualTransmitionManager VisualTransmition;
        public BattleUIShake Shaker;
        public BattleUIManager UI;
        public MinigameManager Minigame;
        public BattleData Data;
        public BattleSpashMessageWriter SpashWriter;
        public SpiderModeGridManager SpiderModeGrid;

        public Canvas Canvas;

        public BattlePipeline Pipeline { get; private set; }

        public BattleConfig Config { get; private set; }

        public BattleUtility Utility { get; private set; }
        public static BattleUtility BattleUtility => Instance.Utility;

        private LocalManager Local => LocalManager.Instance;


        public override void Initialize()
        {
            Instance = this;

            Config = Resources.Load<BattleConfig>("BattleConfig");
            Local.DI.AddSignleton(Config);

            Local.DI.AddSignleton(Data);
            Local.DI.AddSignleton(EnemyModels);
            Local.DI.AddSignleton(BattleAudio);

            InitializeChild();
        }

        public override void InitializeChild()
        {
            Local.DI.AddSignleton(BattleField);
            BattleField.Initialize();

            Local.DI.AddSignleton(EnemyBehaviour);
            EnemyBehaviour.Initialize();

            Pipeline = new BattlePipeline(this, SharedManager.Instance);
            Local.DI.AddSignleton(Pipeline);

            Utility = new BattleUtility(this);
            Local.DI.AddSignleton(Utility);

            Choice.Initialize();

            SpashWriter.Initialize();

            Local.DI.AddSignleton(Player);
            Player.Initialize();
            Player.SetActive(false);

            Local.DI.InjectInto(SpiderModeGrid);
            SpiderModeGrid.Initialize();
        }
    }

}