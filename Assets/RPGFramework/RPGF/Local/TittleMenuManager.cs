using RPGF.Domain.DI;
using RPGF.Explorer.Player;
using RPGF.GUI;
using RPGF.GUI.Abstractions;

public class TittleMenuManager : GUIManagerBase
{
    [Inject]
    private readonly PlayerExplorerManager _playerExplorerManager = null!;

    public override void Initialize()
    {
        base.Initialize();
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
