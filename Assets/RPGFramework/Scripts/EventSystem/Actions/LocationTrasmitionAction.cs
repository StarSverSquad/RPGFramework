using System;
using System.Collections;
using UnityEngine;

public class LocationTrasmitionAction : GraphActionBase
{
    public LocationInfo Location;

    public string SpawnPointName;

    public LocationTrasmitionAction() : base("LocationTrasmition")
    {
        Location = null;
        SpawnPointName = "Default";
    }

    public override IEnumerator ActionCoroutine()
    {
        if (Location != null)
        {
            if (ExplorerManager.Instance.eventHandler.HandledEvent == gameEvent) 
                ExplorerManager.Instance.eventHandler.ForceUnhandle();

            GameManager.Instance.LocationManager.ChangeLocation(Location, SpawnPointName);
        }
            

        yield return new WaitWhile(() => GameManager.Instance.LocationManager.IsChanging);
    }

    public override string GetHeader()
    {
        return "Смена локации";
    }
}