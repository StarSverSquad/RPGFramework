using RPGF.Core.Architecture;
using RPGF.Domain;
using RPGF.Domain.Interfaces;
using RPGF.Explorer.Player;
using UnityEngine;

namespace RPGF.Explorer
{
    public class ExplorerManager : KernelManagerBase
    {
        public static ExplorerManager Instance;

        public ExplorerEventHandler EventHandler;
        public PlayerExplorerManager PlayerManager;
        public ExplorerItemConsumeManager ItemConsumer;

        public static PlayerExplorerMovement PlayerMovement => Instance.PlayerManager.movement;

        public override void Initialize()
        {
            Instance = this;

            InitializeChild();
        }

        public override void InitializeChild()
        {

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