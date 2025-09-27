using RPGF.Battle.Enemy;
using RPGF.Battle.Minigames;
using RPGF.Battle.Player;
using RPGF.Battle.UI;
using RPGF.Core;
using RPGF.RPG;
using RPGF.Shared;

namespace RPGF.Battle
{
    public class BattleManager : KernelManagerBase
    {
        public static BattleManager Instance;

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

        public BattlePipeline Pipeline { get; private set; }

        public BattleUtility Utility { get; private set; }
        public static BattleUtility BattleUtility => Instance.Utility;

        // TRASH
        public static BattleData _Data => Instance.Data;

        public static bool IsBattle => Instance.Pipeline.MainIsWorking;

        public static void StartBattle(RPGBattleInfo info) => BattleUtility.StartBattle(info);
        //

        private LocalManager Local => LocalManager.Instance;

        public override void Initialize()
        {
            Instance = this;

            InitializeChild();
        }

        public override void InitializeChild()
        {
            Pipeline = new BattlePipeline(this, SharedManager.Instance);

            Utility = new BattleUtility(this);
            Local.DI.AddSignleton(Utility);

            Choice.Initialize();

            SpashWriter.Initialize();

            Player.SetActive(false);
        }
    }

}