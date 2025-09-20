using RPGF.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Location
{
    public class LocalLocationManager : MonoBehaviour, IManagerInitialize
    {
        [SerializeField]
        private List<LocationController> Locations = new();

        public LocationController Current => GetLocationByInfo(GameManager.Instance.LocationManager.CurrentLocation);

        public void Initialize()
        {
            GameObject locationContainer = GameObject.FindGameObjectWithTag("LocationsContainer");

            LocationController[] locations = locationContainer.GetComponentsInChildren<LocationController>();

            Locations.AddRange(locations);
        }

        public LocationController GetLocationByInfo(RpgfLocationInfo info)
        {
            return Locations.FirstOrDefault(i => i.Info == info);
        }
    }
}