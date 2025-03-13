using UnityEngine;

/// <summary>
/// Global game boostrap class
/// </summary>
public class GameManager : ContentManagerBase
{
    public static GameManager Instance;

    [Header("Глобальные ссылки")]
    public GlobalLocationManager LocationManager;
    public AudioManager GameAudio;
    public LoadingScreenManager LoadingScreen;
    public SceneLoadManager SceneLoader;

    [SerializeField, Space]
    private LocationInfo newGameLocation;

    public LocalizationManager Localization {  get; private set; }
    public InventoryManager Inventory { get; private set; }
    public GlobalCharacterManager Character { get; private set; }
    public SaveLoadManager SaveLoad { get; private set; }
    public BaseOptions BaseOptions { get; private set; }
    public GameData GameData { get; private set; }
    public GameConfigManager GameConfig { get; private set; }

    public static LocalizationManager ILocalization => Instance.Localization;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            NewGame();
    }

    public override void InitializeChild()
    {
        Character = new GlobalCharacterManager();

        Inventory = new InventoryManager();

        BaseOptions = Resources.Load<BaseOptions>("Options");

        GameData = new GameData(this);

        SaveLoad = new SaveLoadManager(this);

        GameConfig = new GameConfigManager(SaveLoad);

        GameConfig.Load();
        GameConfig.Apply();

        Localization = new LocalizationManager();
    }

    public void NewGame()
    {
        Inventory.Dispose();
        Character.Dispose();
        GameData.Dispose();

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        LocationManager.ChangeLocation(newGameLocation);
    }
    public void LoadGame(int slotId)
    {
        SaveLoad.Load(slotId);
    }
}
