using RPGF.Battle;
using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Core.Location;
using RPGF.Domain.DI;
using RPGF.Explorer;
using RPGF.GUI;
using RPGF.Overworld;
using RPGF.Shared;
using UnityEngine;

namespace RPGF
{
    public class LocalManager : KernelManagerBase
    {
        public static LocalManager Instance;

        [Header("Общие ссылки")]
        public MainCameraManager Camera;
        public LocalLocationManager Location;
        public TittleMenuManager TittleMenu;

        [Space]

        [Header("Ссылки для инициализации")]
        [SerializeField]
        private OverworldManager overworld;
        [SerializeField]
        private SharedManager shared;
        [SerializeField]
        private BattleManager battle;

        public DependencyInjection DI { get; private set; }

        public override void Initialize()
        {
            Instance = this;

            DI = new DependencyInjection();

            DI.AddSignleton(DI);
            DI.AddSubInjector(Game.DI);

            InitializeChild();
        }

        public void Start()
        {
            Game.LocalInitializeRequest(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(GlobalManager.Instance.BaseOptions.Additional)
                && !TittleMenu.IsOpened
                && !overworld.EventHandler.EventPlaying)
                TittleMenu.Open();
        }

        public override void InitializeChild()
        {
            shared.Initialize();

            overworld.Initialize();

            DI.AddSignleton(Camera);
            Camera.Initialize();
            DI.AddSignleton(Location);
            Location.Initialize();
            DI.AddSignleton(TittleMenu);
            TittleMenu.Initialize();

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