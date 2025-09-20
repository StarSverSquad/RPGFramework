using RPGF.RPG;
using RPGF.Shared;

public class BattleManager : ContentManagerBase, IManagerInitialize
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

    public void Initialize()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        Pipeline = new BattlePipeline(this, SharedManager.Instance);
        Utility = new BattleUtility(this);

        Choice.Initialize();

        SpashWriter.Initialize();

        Player.SetActive(false);
    }
}
