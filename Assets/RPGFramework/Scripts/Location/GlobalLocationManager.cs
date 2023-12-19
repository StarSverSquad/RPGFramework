using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalLocationManager : MonoBehaviour
{
    public event Action<LocationInfo> OnLocationChanged;

    public LocationInfo CurrentLocation = null;

    [SerializeField]
    private bool isChanging = false;
    public bool IsChanging => isChanging;

    private void Start()
    {
        /// DEBUG
        ChangeLocation(CurrentLocation);
    }

    public void ChangeLocation(LocationInfo location, string pointName = "")
    {
        StartCoroutine(ChangeLocationCoroutine(location, pointName));
    }

    public LocationInfo LoadLocationInfoByFileName(string fileName)
    {
        return Resources.Load<LocationInfo>($"Locations/{fileName}");
    }
    public LocationInfo LoadLocationInfoByName(string locName)
    {
        LocationInfo[] locations =  Resources.LoadAll<LocationInfo>($"Locations");

        return locations.First(i => i.Name == locName);
    }

    private IEnumerator ChangeLocationCoroutine(LocationInfo location, string pointName = "default")
    {
        isChanging = true;

        GameManager.Instance.loadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        if (CurrentLocation != null && location.SceneName == SceneManager.GetActiveScene().name)
            ExplorerManager.instance.locationManager.GetLocationByInfo(CurrentLocation).OnLeave();

        if (location.SceneName != SceneManager.GetActiveScene().name)
        {
            GameManager.Instance.sceneLoader.LoadScene(location.SceneName);

            yield return new WaitWhile(() => GameManager.Instance.sceneLoader.IsLoading);
        }

        if (ExplorerManager.instance != null)
        {
            LocationObject obj = ExplorerManager.instance.locationManager.GetLocationByInfo(location);

            obj.OnEnter();

            if (obj.SpawnPoints.Count() == 0)
            {
                Debug.LogError("” локации не задоны точки спавна!");

                yield break;
            }

            LocationSpawnPoint spawnPoint = obj.SpawnPoints.FirstOrDefault(obj => obj.Name == pointName)
                                            ?? obj.SpawnPoints[0];

            ExplorerManager.instance.playerManager.TeleportToVector(spawnPoint.transform.position);

            ExplorerManager.instance.playerManager.movement.RotateTo(spawnPoint.SpawnDirection);

            ExplorerManager.instance.characterManager.UpdateModels();


            switch (location.CameraLink)
            {
                case MainCameraManager.CameraLink.Player:
                    ExplorerManager.instance.cameraManager.SetPlayer();
                    break;
                case MainCameraManager.CameraLink.LocationPoint:
                    ExplorerManager.instance.cameraManager.SetLocationPoint(obj);
                    break;
                case MainCameraManager.CameraLink.PlayerFollow:
                    ExplorerManager.instance.cameraManager.SetPlayerFollow();
                    break;
            }
        }
        else
        {
            Debug.LogError("ExplorerManager не найден!");

            yield break;
        }

        CurrentLocation = location;
        OnLocationChanged?.Invoke(location);

        GameManager.Instance.loadingScreen.DeactivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        isChanging = false;
    }
}
