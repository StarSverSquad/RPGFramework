using RPGF.Battle;
using RPGF.Core.Architecture;
using RPGF.Core.Character;
using RPGF.Core.Location;
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

        public override void Initialize()
        {
            
        }

        public void Start()
        {
            Instance = this;

            InitializeChild();
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
            TittleMenu.Initialize();

            explorer.Initialize();

            Location.Initialize();
            Character.Initialize();

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