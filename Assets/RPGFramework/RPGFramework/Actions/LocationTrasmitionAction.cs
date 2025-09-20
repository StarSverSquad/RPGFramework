using System;
using System.Collections;
using UnityEngine;

public class LocationTrasmitionAction : GraphActionBase
{
    public GlobalLocationManager.TransimitionMessage Message;

    public LocationTrasmitionAction() : base("LocationTrasmition")
    {
        Message = new GlobalLocationManager.TransimitionMessage();
    }

    public override IEnumerator ActionCoroutine()
    {
        if (Message.Location != null)
        {
            if (ExplorerManager.Instance.EventHandler.HandledEvent == gameEvent) 
                ExplorerManager.Instance.EventHandler.ForceUnhandle();

            ExplorerManager.PlayerMovement.CanWalk = false;

            GameManager.Instance.LocationManager.ChangeLocation(Message);
        }     

        yield return new WaitWhile(() => GameManager.Instance.LocationManager.IsChanging);

        ExplorerManager.PlayerMovement.CanWalk = true;
    }

    public override string GetHeader()
    {
        return "Смена локации";
    }
}