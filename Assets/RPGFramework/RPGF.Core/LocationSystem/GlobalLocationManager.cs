using System;
using System.Collections;
using System.Linq;
using RPGF.Core.Enums;
using RPGF.Domain;
using RPGF.Domain.DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGF.Core.Location
{
    public class GlobalLocationManager : RPGFrameworkBehaviour
    {
        [Inject]
        private readonly LoadingScreenManager _loadingScreen = null!;
        [Inject]
        private readonly SceneLoadManager _sceneLoader = null!;

        public RpgfLocationInfo CurrentLocation { get; private set; } = null;
        public bool IsChanging => changingCoroutine != null;

        public event Action<RpgfLocationInfo> OnLocationChanged;

        private Coroutine changingCoroutine = null;

        #region API

        public void ChangeLocation(RpgfLocationInfo location, string pointName = "default")
        {
            if (IsChanging || location == null)
                return;

            LocationTransimitionDto message = new()
            {
                Location = location,
                Point = pointName,
                TeleportToPoint = true,
                Direction = ViewDirection.Down,
                Position = Vector2.zero
            };

            changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
        }
        public void ChangeLocation(RpgfLocationInfo location, Vector2 position, ViewDirection direction)
        {
            if (IsChanging) return;

            LocationTransimitionDto message = new()
            {
                Location = location,
                Point = "default",
                TeleportToPoint = false,
                Direction = direction,
                Position = position
            };

            changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
        }
        public void ChangeLocation(LocationTransimitionDto message)
        {
            if (IsChanging) return;

            changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
        }

        public RpgfLocationInfo LoadLocationInfoByFileName(string fileName)
        {
            return Resources.Load<RpgfLocationInfo>($"Locations/{fileName}");
        }
        public RpgfLocationInfo LoadLocationInfoByTag(string locationTag)
        {
            var locations = Resources.LoadAll<RpgfLocationInfo>($"Locations");

            return locations.FirstOrDefault(i => i.Tag == locationTag);
        }

        #endregion

        private IEnumerator ChangeLocationCoroutine(LocationTransimitionDto message)
        {
            _loadingScreen.ShowBackground();

            yield return new WaitWhile(() => _loadingScreen.IsBackgroundFading);

            if (CurrentLocation != null && message.Location.SceneName == SceneManager.GetActiveScene().name)
                Local.Location.GetLocationByInfo(CurrentLocation).OnLeave();

            if (message.Location.SceneName != SceneManager.GetActiveScene().name)
            {
                _loadingScreen.ShowProggresBar();

                _sceneLoader.LoadScene(message.Location.SceneName);

                yield return new WaitWhile(() => _sceneLoader.IsLoading);
            }

            if (!Local)
            {
                Debug.LogError("LocalManager не найден!");
                yield break;
            }

            var location = Local.Location.GetLocationByInfo(message.Location);

            location.OnEnter();

            if (message.TeleportToPoint)
            {
                if (location.SpawnPoints.Count() == 0)
                {
                    Debug.LogError("У локации нет точки спавна!");
                    yield break;
                }

                var spawnPoint = location.SpawnPoints.FirstOrDefault(obj => obj.Name == message.Point)
                                                     ?? location.SpawnPoints[0];

                Explorer.PlayerManager.TeleportToVector(spawnPoint.transform.position);

                Explorer.PlayerManager.movement.RotateTo(spawnPoint.SpawnDirection);
            }
            else
            {
                Explorer.PlayerManager.TeleportToVector(message.Position);

                Explorer.PlayerManager.movement.RotateTo(message.Direction);
            }

            Local.Character.RebuildModels();

            switch (message.Location.CameraCapture)
            {
                case MainCameraManager.CaptureType.Player:
                    Local.Camera.PlaceToPlayer();
                    break;
                case MainCameraManager.CaptureType.LocationPoint:
                    Local.Camera.PlaceToLocationPoint(location);
                    break;
                case MainCameraManager.CaptureType.PlayerFollow:
                    Local.Camera.PlaceToLocationPoint(location);

                    Local.Camera.FollowToPlayer();
                    break;
                default:
                    Local.Camera.PlaceToPlayer();
                    break;
            }

            CurrentLocation = message.Location;

            OnLocationChanged?.Invoke(message.Location);

            _loadingScreen.HideBackground();
            _loadingScreen.HideProggresBar();

            yield return new WaitWhile(() => _loadingScreen.IsBackgroundFading);

            changingCoroutine = null;
        }
    }
}
