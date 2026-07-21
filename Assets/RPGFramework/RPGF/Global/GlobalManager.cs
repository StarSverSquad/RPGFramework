using RPGF.Core;
using DG.Tweening;
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

        public GameUtils Utils { get; private set; }

        public static LocalizationService ILocalization => Instance.Localization;

        public override void Initialize()
        {
            Instance = this;

            Game.DI.AddSingleton(LoadingScreen);
            Game.DI.AddSingleton(GameAudio);
            Game.DI.AddSingleton(SceneLoader);
            Game.DI.AddSingleton(LocationManager);

            InitializeChild();
        }

        public override void InitializeChild()
        {
            FilesService = Game.DI.CreateSingleton<GameFilesService>();

            Character = Game.DI.CreateSingleton<CharacterService>();

            Inventory = Game.DI.CreateSingleton<InventoryService>();

            BaseOptions = Resources.Load<BaseOptions>("Options");
            Game.DI.AddSingleton(BaseOptions);

            GameData = Game.DI.CreateSingleton<GameData>();
            GameData.Initialize();

            GameConfig = Game.DI.CreateSingleton<GameConfigService>();
            GameConfig.LoadAndApply();

            CommonDataService = Game.DI.CreateSingleton<GameCommonDataService>();
            CommonDataService.Load();

            FastSave = Game.DI.CreateSingleton<FastSaveService>();

            Localization = Game.DI.CreateSingleton<LocalizationService>();

            SaveLoad = Game.DI.CreateSingleton<SaveLoadService>();

            Utils = Game.DI.CreateSingleton<GameUtils>();
        }

        private void OnApplicationQuit()
        {
            DOTween.KillAll();

            GameConfig.Save();
            CommonDataService.Save();
        }
    }
}