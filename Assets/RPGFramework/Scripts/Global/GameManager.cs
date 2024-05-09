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

    public InventoryManager Inventory { get; private set; }
    public GlobalCharacterManager Character { get; private set; }
    public SaveLoadManager SaveLoad { get; private set; }
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
        Character = new GlobalCharacterManager();

        Inventory = new InventoryManager();

        GameConfig = Resources.Load<GameConfig>("Config");

        GameData = new GameData(this);

        SaveLoad = new SaveLoadManager();
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
