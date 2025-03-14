using RPGF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalLocationManager : MonoBehaviour
{
    [Serializable]
    public class TransimitionMessage
    {
        public LocationInfo Location;

        public string Point;

        public bool TeleportToPoint;

        public Vector2 Position;
        public ViewDirection Direction;

        public TransimitionMessage()
        {
            Location = null;
            Point = string.Empty;
            TeleportToPoint = true;
            Position = Vector2.zero;
            Direction = ViewDirection.Down;
        }
    }

    public event Action<LocationInfo> OnLocationChanged;

    public LocationInfo CurrentLocation = null;

    private Coroutine changingCoroutine = null;
    public bool IsChanging => changingCoroutine != null;

    /// <summary>
    /// Переносит игрока в лакацию к заготовленой точке
    /// </summary>
    /// <param name="location">Локация</param>
    /// <param name="pointName">Название точки</param>
    public void ChangeLocation(LocationInfo location, string pointName = "default")
    {
        if (IsChanging || location == null) 
            return;

        TransimitionMessage message = new TransimitionMessage()
        {
            Location = location,
            Point = pointName,
            TeleportToPoint = true,
            Direction = ViewDirection.Down,
            Position = Vector2.zero
        };

        changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
    }
    /// <summary>
    /// Переносит игрока в лакацию к определённой позиции на сцене
    /// </summary>
    /// <param name="location">Локация</param>
    /// <param name="position">Позиция на сцене</param>
    /// <param name="direction">Направление взгляда</param>
    public void ChangeLocation(LocationInfo location, Vector2 position, ViewDirection direction)
    {
        if (IsChanging) return;

        TransimitionMessage message = new TransimitionMessage()
        {
            Location = location,
            Point = "default",
            TeleportToPoint = false,
            Direction = direction,
            Position = position
        };

        changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
    }
    /// <summary>
    /// Переносит игрока в лакцию по сответсвии с заданными параметрами в TransimitionMessage
    /// </summary>
    public void ChangeLocation(TransimitionMessage message)
    {
        if (IsChanging) return;

        changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
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

    private IEnumerator ChangeLocationCoroutine(TransimitionMessage message)
    {
        GameManager.Instance.LoadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.LoadingScreen.BgIsFade);

        if (CurrentLocation != null && message.Location.SceneName == SceneManager.GetActiveScene().name)
            LocalManager.Instance.Location.GetLocationByInfo(CurrentLocation).OnLeave();

        if (message.Location.SceneName != SceneManager.GetActiveScene().name)
        {
            GameManager.Instance.SceneLoader.LoadGameScene(message.Location.SceneName);

            yield return new WaitWhile(() => GameManager.Instance.SceneLoader.IsLoading);
        }

        if (LocalManager.Instance != null)
        {
            LocationObject obj = LocalManager.Instance.Location.GetLocationByInfo(message.Location);

            obj.OnEnter();

            if (message.TeleportToPoint)
            {
                if (obj.SpawnPoints.Count() == 0)
                {
                    Debug.LogError("У локации нет точки спавна!");

                    yield break;
                }

                LocationSpawnPoint spawnPoint = obj.SpawnPoints.FirstOrDefault(obj => obj.Name == message.Point)
                                                ?? obj.SpawnPoints[0];

                ExplorerManager.Instance.PlayerManager.TeleportToVector(spawnPoint.transform.position);

                ExplorerManager.Instance.PlayerManager.movement.RotateTo(spawnPoint.SpawnDirection);
            }
            else
            {
                ExplorerManager.Instance.PlayerManager.TeleportToVector(message.Position);

                ExplorerManager.Instance.PlayerManager.movement.RotateTo(message.Direction);
            }

            LocalManager.Instance.Character.UpdateModels();

            switch (message.Location.CameraLink)
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
            Debug.LogError("LocalManager не найден!");

            yield break;
        }

        CurrentLocation = message.Location;

        OnLocationChanged?.Invoke(message.Location);

        GameManager.Instance.LoadingScreen.DeactivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.LoadingScreen.BgIsFade);

        changingCoroutine = null;
    }
}
