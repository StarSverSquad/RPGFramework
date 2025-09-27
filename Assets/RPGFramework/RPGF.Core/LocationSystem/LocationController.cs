using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.Core.Location
{
    public class LocationController : RPGFrameworkBehaviour
    {
        public UnityEvent OnEnterLocation;
        public UnityEvent OnLeaveLocation;

        public RpgfLocationInfo Info;

        public List<LocationSpawnPoint> SpawnPoints = new();

        public Transform CameraPoint;
        public GameObject MapContainer;

        private void Awake()
        {
            SpawnPoints.Clear();

            SpawnPoints.AddRange(GetComponentsInChildren<LocationSpawnPoint>());
        }

        private void Start()
        {
            MapContainer.SetActive(false);
        }

        public void OnEnter()
        {
            MapContainer.SetActive(true);

            OnEnterLocation?.Invoke();
        }

        public void OnLeave()
        {
            MapContainer.SetActive(false);

            OnLeaveLocation?.Invoke();
        }
    }
}