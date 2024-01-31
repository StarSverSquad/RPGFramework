using UnityEngine;

public class GameManager : ContentManagerBase
{
    public static GameManager Instance;

    [Header("Глобальные ссылки")]
    public InventoryManager inventory;
    public GlobalCharacterManager character;
    public GlobalLocationManager locationManager;
    public AudioManager gameAudio;
    public LoadingScreenManager loadingScreen;
    public SceneLoadManager sceneLoader;
    public SaveLoadManager saveLoad;

    public GameConfig GameConfig { get; private set; }

    public GameData GameData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            InitializeChild();
        }
        else
            Destroy(gameObject);
    }

    public override void InitializeChild()
    {
        GameConfig = Resources.Load<GameConfig>("Config");
        GameData = new GameData(this);

        character.Initialize();
    }
}
