using RPGF.GUI;

public class TittleMenuManager : GUIManagerBase
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnOpen()
    {
        Explorer.PlayerManager.movement.SetMovementAccess(false);
    }

    public override void OnClose()
    {
        Explorer.PlayerManager.movement.SetMovementAccess(true);
    }
}
