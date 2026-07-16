using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI.Abstractions;
using RPGF.Overworld.Player;
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
        private readonly PlayerOverworldManager _playerManager = null!;

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
            _playerManager.movement.SetMovementAccess(false);
            _playerManager.interaction.CanInteract = false;
        }

        public override void OnClose()
        {
            _playerManager.movement.SetMovementAccess(true);
            _playerManager.interaction.CanInteract = true;
        }
    }
}
