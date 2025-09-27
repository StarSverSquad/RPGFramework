using RPGF.Battle;
using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Core.Location;
using RPGF.Domain.DI;
using RPGF.Explorer;
using RPGF.Shared;
using UnityEngine;

namespace RPGF
{
    public class LocalManager : KernelManagerBase
    {
        public static LocalManager Instance;

        [Header("Общие ссылки")]
        public MainCameraManager Camera;
        public CharacterManager Character;
        public LocalLocationManager Location;
        public TittleMenuManager TittleMenu;
        public SunManager Sun;

        [Space]

        [Header("Ссылки для инициализации")]
        [SerializeField]
        private ExplorerManager explorer;
        [SerializeField]
        private SharedManager common;
        [SerializeField]
        private BattleManager battle;

        public DependencyInjection DI { get; private set; }

        public override void Initialize()
        {
            Instance = this;

            DI = new DependencyInjection();

            DI.AddSubInjector(Game.DI);

            DI.AddSignleton(Sun);

            InitializeChild();
        }

        public void Awake()
        {
            Game.LocalInitializeRequest(this);
        }

        private void Update()
        {
            /// 0_o
            if (Input.GetKeyDown(GlobalManager.Instance.BaseOptions.Additional)
                && !TittleMenu.IsOpened
                && !explorer.EventHandler.EventRuning)
                TittleMenu.Open();
        }

        public override void InitializeChild()
        {
            Camera.Initialize();
            DI.AddSignleton(Camera);

            TittleMenu.Initialize();
            DI.AddSignleton(TittleMenu);

            explorer.Initialize();

            Location.Initialize();
            DI.AddSignleton(Location);

            Character.Initialize();
            DI.AddSignleton(Character);

            common.Initialize();

            battle.Initialize();
        }

        public static LocationController GetCurrentLocation()
        {
            if (Instance == null)
                return null;

            return Instance.Location.Current;
        }
    }
}