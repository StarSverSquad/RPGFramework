using System.Collections.Generic;
using RPGF;
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

        private void OnDrawGizmos()
        {
            if (CameraPoint == null)
                return;

            Camera camera = Camera.main;
            if (camera == null || !camera.orthographic)
                return;

            float height = camera.orthographicSize * 2f;
            float width = height * camera.aspect;

            Gizmos.color = new Color(0f, 1f, 1f, 0.8f);
            Gizmos.DrawWireCube(CameraPoint.position, new Vector3(width, height, 0f));

            if (Info != null && Info.CameraCapture == MainCameraManager.CaptureType.PlayerFollow)
            {
                Vector2 border = Info.PlayerFollowBorder;
                Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
                Gizmos.DrawWireCube(
                    CameraPoint.position,
                    new Vector3(border.x * 2f, border.y * 2f, 0f));
            }
        }
    }
}