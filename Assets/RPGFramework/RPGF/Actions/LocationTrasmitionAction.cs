using RPGF;
using RPGF.Core.Location;
using RPGF.EventSystem;
using RPGF.Explorer;
using System.Collections;
using UnityEngine;

public class LocationTrasmitionAction : ActionBase
{
    public LocationTransimitionDto Message;

    public LocationTrasmitionAction() : base("LocationTrasmition")
    {
        Message = new LocationTransimitionDto();
    }

    public override IEnumerator ActionCoroutine()
    {
        if (Message.Location != null)
        {
            if (ExplorerManager.Instance.EventHandler.HandledEvent == gameEvent) 
                ExplorerManager.Instance.EventHandler.ForceUnhandle();

            ExplorerManager.PlayerMovement.CanWalk = false;

            GlobalManager.Instance.LocationManager.ChangeLocation(Message);
        }     

        yield return new WaitWhile(() => GlobalManager.Instance.LocationManager.IsChanging);

        ExplorerManager.PlayerMovement.CanWalk = true;
    }

    public override string GetHeader()
    {
        return "Смена локации";
    }
}