using Assets.RPGFramework.RPGF.Battle;
using RPGF.Core.Battle;
using RPGF.Battle.Enemy;
using RPGF.Battle.Player;
using RPGF.Battle.UI;
using RPGF.Core;
using RPGF.Shared;
using UnityEngine;
using RPGF.Battle.Choice;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Projectiles;
using RPGF.Core.Battle.Behaviour;

namespace RPGF.Battle
{
    public class BattleManager : KernelManagerBase
    {
        public static BattleManager Instance;
        public static bool IsBattle => Instance.Pipeline.MainIsWorking;

        public BattleChoiceManager Choice;
        public BattleFieldManager BattleField;
        public ProjectileManager Projectiles;
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
            Local.DI.AddSingleton(Config);

            Local.DI.AddSingleton(Data);
            Local.DI.AddSingleton(EnemyModels);
            Local.DI.AddSingleton(BattleAudio);

            InitializeChild();
        }

        public override void InitializeChild()
        {
            Local.DI.AddSingleton(BattleField);
            BattleField.Initialize();

            Local.DI.AddSingleton(Projectiles);
            Projectiles.Initialize();

            Local.DI.AddSingleton(EnemyBehaviour);
            EnemyBehaviour.Initialize();

            Pipeline = new BattlePipeline(this, SharedManager.Instance);
            Local.DI.AddSingleton(Pipeline);

            Utility = new BattleUtility(this);
            Local.DI.AddSingleton(Utility);

            Choice.Initialize();

            SpashWriter.Initialize();

            Local.DI.AddSingleton(Player);
            Player.Initialize();
            Player.SetActive(false);
        }
    }
}