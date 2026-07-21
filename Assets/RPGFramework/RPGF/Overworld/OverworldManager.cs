using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Core.Enums;
using RPGF.Explorer;
using RPGF.Overworld.Player;
using UnityEngine;

namespace RPGF.Overworld
{
    public class OverworldManager : KernelManagerBase
    {
        public static OverworldManager Instance;
        public static PlayerOverworldMovement PlayerMovement => Instance.PlayerManager.movement;

        public OverworldEventHandler EventHandler;
        public PlayerOverworldManager PlayerManager;
        public CharacterManager CharacterManager;
        public SunManager SunManager;

        public OverworldItemConsumeService ItemConsumeService { get; private set; }
        public OverworldDamageService DamageService { get; private set; }

        private LocalManager Local => LocalManager.Instance;

        public override void Initialize()
        {
            Instance = this;

            InitializeChild();
        }

        public override void InitializeChild()
        {
            Local.DI.AddSingleton(EventHandler);
            Local.DI.AddSingleton(SunManager);

            Local.DI.AddSingleton(PlayerManager);
            CharacterManager.Initialize();
            Local.DI.AddSingleton(CharacterManager);

            ItemConsumeService = Local.DI.CreateSingleton<OverworldItemConsumeService>();
            DamageService = Local.DI.CreateSingleton<OverworldDamageService>();
        }

        public static Vector2 GetPlayerPosition()
        {
            if (Instance == null)
                return Vector2.zero;

            return Instance.PlayerManager.transform.position;
        }
        public static Vector3 GetPlayerPosition3D()
        {
            if (Instance == null)
                return Vector3.zero;

            return Instance.PlayerManager.transform.position;
        }

        public static ViewDirection GetPlayerViewDirection()
        {
            return Instance.PlayerManager.movement.ViewDirection;
        }
    }
}