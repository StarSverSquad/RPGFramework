using RPGF.Core;
using RPGF.Core.Enums;
using RPGF.Domain;
using RPGF.Explorer.Player;
using UnityEngine;

namespace RPGF.Explorer
{
    public class ExplorerManager : KernelManagerBase
    {
        public static ExplorerManager Instance;
        public static PlayerExplorerMovement PlayerMovement => Instance.PlayerManager.movement;

        public ExplorerEventHandler EventHandler;
        public PlayerExplorerManager PlayerManager;
        public ExplorerItemConsumeManager ItemConsumer;

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