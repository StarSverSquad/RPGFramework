using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.Overworld.Player;
using RPGF.GUI.Abstractions;
using UnityEngine;

namespace RPGF.GUI
{
    public class TittleMenuManager : GUIManagerBase
    {
        [Inject]
        private readonly BaseOptions _options = null!;
        [Inject]
        private readonly SceneLoadManager _sceneLoader = null!;
        [Inject]
        private readonly PlayerOverworldManager _playerExplorerManager = null!;

        [SerializeField]
        private GUIBlock saveLoadGUIBlock;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void OpenSaveMenu()
        {
            if (saveLoadGUIBlock is ISaveLoadGUIBlock saveLoad)
            {
                saveLoad.SetSaveMode(true);
                Open(saveLoad);
            }
        }

        public void ExitToMenu()
        {
            _sceneLoader.LoadScene(_options.MainMenuScene);
        }

        public void ExitToOS()
        {
            Application.Quit();
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
