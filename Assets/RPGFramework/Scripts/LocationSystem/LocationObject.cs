using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LocationObject : MonoBehaviour
{
    public UnityEvent OnEnterLocation;
    public UnityEvent OnLeaveLocation;

    public LocationInfo Info;

    public List<LocationSpawnPoint> SpawnPoints = new List<LocationSpawnPoint>();

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
