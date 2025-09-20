using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalLocationManager : MonoBehaviour, IManagerInitialize
{
    [SerializeField]
    private List<LocationController> Locations = new List<LocationController>();

    public LocationController Current => GetLocationByInfo(GameManager.Instance.LocationManager.CurrentLocation);

    public void Initialize()
    {
        GameObject locationContainer = GameObject.FindGameObjectWithTag("LocationsContainer");

        LocationController[] locations = locationContainer.GetComponentsInChildren<LocationController>();

        Locations.AddRange(locations);
    }

    public LocationController GetLocationByInfo(LocationInfo info)
    {
        return Locations.FirstOrDefault(i => i.Info == info);
    }
}
