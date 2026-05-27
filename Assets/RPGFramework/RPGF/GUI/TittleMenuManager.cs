using RPGF.Domain.DI;
using RPGF.Explorer.Player;
using RPGF.GUI;
using RPGF.GUI.Abstractions;
using UnityEngine;

namespace RPGF.GUI
{
    public class TittleMenuManager : GUIManagerBase
    {
        [Inject]
        private readonly PlayerExplorerManager _playerExplorerManager = null!;

        [SerializeField]
        private GUIBlock saveLoadGUIBlock;

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Update()
        {
            // TODO: Remove this after testing
            if (Input.GetKeyDown(KeyCode.S))
            {
                OpenSaveMenu();
            }
        }

        public void OpenSaveMenu()
        {
            if (saveLoadGUIBlock is ISaveLoadGUIBlock saveLoad)
            {
                saveLoad.SetSaveMode(true);
                Open(saveLoad);
            }
        }

        public override void OnOpen()
        {
            _playerExplorerManager.movement.SetMovementAccess(false);
            _playerExplorerManager.interaction.CanInteract = false;
        }

        public override void OnClose()
        {
            _playerExplorerManager.movement.SetMovementAccess(true);
            _playerExplorerManager.interaction.CanInteract = true;
        }
    }
}
