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

    public LocationObject Current => GetLocationByInfo(GameManager.Instance.locationManager.CurrentLocation);

    public void Initialize()
    {
        LocationObject[] locations = ExplorerManager.Instance.GetComponentsInChildren<LocationObject>();

        Locations.AddRange(locations);
    }

    public LocationObject GetLocationByInfo(LocationInfo info)
    {
        return Locations.FirstOrDefault(i => i.Info == info);
    }
}
