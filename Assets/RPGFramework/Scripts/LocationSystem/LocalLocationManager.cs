using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalLocationManager : MonoBehaviour, IManagerInitialize
{
    [SerializeField]
    private List<LocationObject> Locations = new List<LocationObject>();

    public LocationObject Current => GetLocationByInfo(GameManager.Instance.LocationManager.CurrentLocation);

    public void Initialize()
    {
        GameObject locationContainer = GameObject.FindGameObjectWithTag("LocationsContainer");

        LocationObject[] locations = locationContainer.GetComponentsInChildren<LocationObject>();

        Locations.AddRange(locations);
    }

    public LocationObject GetLocationByInfo(LocationInfo info)
    {
        return Locations.FirstOrDefault(i => i.Info == info);
    }
}
