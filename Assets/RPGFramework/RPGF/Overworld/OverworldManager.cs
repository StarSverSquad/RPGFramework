using RPGF.Core;
using RPGF.Core.Enums;
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
        public OverworldItemConsumeManager ItemConsumer;

        private LocalManager Local => LocalManager.Instance;

        public override void Initialize()
        {
            Instance = this;

            Local.DI.AddSignleton(EventHandler);
            Local.DI.AddSignleton(PlayerManager);
            Local.DI.AddSignleton(ItemConsumer);
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