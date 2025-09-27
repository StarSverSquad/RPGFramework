using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Location
{
    public class LocalLocationManager : RPGFrameworkBehaviour
    {
        [SerializeField]
        private List<LocationController> Locations = new();

        public LocationController Current => GetLocationByInfo(GlobalManager.Instance.LocationManager.CurrentLocation);

        public override void Initialize()
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