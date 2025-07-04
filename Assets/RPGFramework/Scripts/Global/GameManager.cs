using DG.Tweening;
using RPGF.Inventory;
using RPGF.Localization;
using RPGF.SaveLoad;
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

    public LocalizationService Localization {  get; private set; }
    public InventoryService Inventory { get; private set; }
    public CharacterService Character { get; private set; }
    public SaveLoadService SaveLoad { get; private set; }
    public GameConfigService GameConfig { get; private set; }

    public GameFilesService FilesService { get; private set; }

    public BaseOptions BaseOptions { get; private set; }
    public GameData GameData { get; private set; }

    public static LocalizationService ILocalization => Instance.Localization;

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
        FilesService = new GameFilesService();

        Character = new CharacterService();

        Inventory = new InventoryService();

        BaseOptions = Resources.Load<BaseOptions>("Options");

        GameData = new GameData(this);

        SaveLoad = new SaveLoadService(this, FilesService);

        GameConfig = new GameConfigService(SaveLoad);

        GameConfig.Load();
        GameConfig.Apply();

        Localization = new LocalizationService(GameConfig);
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
        SaveLoad.GameLoad(slotId);
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
