using DG.Tweening;
using RPGF.Core.Architecture;
using RPGF.Core.Character;
using RPGF.Core.Inventory;
using RPGF.Core.Localization;
using RPGF.Core.Location;
using RPGF.Core.SaveLoad;
using UnityEngine;

namespace RPGF
{
    public class GlobalManager : KernelManagerBase
    {
        public static GlobalManager Instance;

        [Header("Глобальные ссылки")]
        public GlobalLocationManager LocationManager;
        public AudioManager GameAudio;
        public SceneLoadManager SceneLoader;
        public LoadingScreenManager LoadingScreen;

        public LocalizationService Localization { get; private set; }
        public InventoryService Inventory { get; private set; }
        public CharacterService Character { get; private set; }
        public SaveLoadService SaveLoad { get; private set; }
        public GameConfigService GameConfig { get; private set; }
        public GameCommonDataService CommonDataService { get; private set; }
        public GameFilesService FilesService { get; private set; }
        public FastSaveService FastSave { get; private set; }

        public BaseOptions BaseOptions { get; private set; }
        public GameData GameData { get; private set; }

        public GameUtils Utils => new GameUtils(this);

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

        public override void Initialize()
        {
            
        }

        public override void InitializeChild()
        {
            FilesService = new GameFilesService();

            Character = new CharacterService();

            Inventory = new InventoryService();

            BaseOptions = Resources.Load<BaseOptions>("Options");

            GameData = new GameData(this);

            GameConfig = new GameConfigService(FilesService, GameAudio);
            GameConfig.LoadAndApply();

            CommonDataService = new GameCommonDataService(FilesService);
            CommonDataService.Load();

            FastSave = new FastSaveService(CommonDataService);

            Localization = new LocalizationService(GameConfig);

            SaveLoad = new SaveLoadService(Character, GameData, LocationManager, Inventory, FilesService, CommonDataService);
        }

        private void OnApplicationQuit()
        {
            DOTween.KillAll();

            GameConfig.Save();
            CommonDataService.Save();
        }
    }
}