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
            LocalManager.Instance.Location.GetLocationByInfo(CurrentLocation).OnLeave();

        if (location.SceneName != SceneManager.GetActiveScene().name)
        {
            GameManager.Instance.sceneLoader.LoadScene(location.SceneName);

            yield return new WaitWhile(() => GameManager.Instance.sceneLoader.IsLoading);
        }

        if (ExplorerManager.Instance != null)
        {
            LocationObject obj = LocalManager.Instance.Location.GetLocationByInfo(location);

            obj.OnEnter();

            if (obj.SpawnPoints.Count() == 0)
            {
                Debug.LogError("У локации нет точки спавна!");

                yield break;
            }

            LocationSpawnPoint spawnPoint = obj.SpawnPoints.FirstOrDefault(obj => obj.Name == pointName)
                                            ?? obj.SpawnPoints[0];

            ExplorerManager.Instance.playerManager.TeleportToVector(spawnPoint.transform.position);

            ExplorerManager.Instance.playerManager.movement.RotateTo(spawnPoint.SpawnDirection);

            LocalManager.Instance.Character.UpdateModels();


            switch (location.CameraLink)
            {
                case MainCameraManager.CameraLink.Player:
                    LocalManager.Instance.Camera.SetPlayer();
                    break;
                case MainCameraManager.CameraLink.LocationPoint:
                    LocalManager.Instance.Camera.SetLocationPoint(obj);
                    break;
                case MainCameraManager.CameraLink.PlayerFollow:
                    LocalManager.Instance.Camera.SetPlayerFollow();
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
