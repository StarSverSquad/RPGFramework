using RPGF.Core.Architecture;
using RPGF.Domain;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGF.Core.Location
{
    public class GlobalLocationManager : GameSystemBase
    {
        /// TODO: Надо вынести в отдльный файл
        #region TransimitionMessage
        [Serializable]
        public class TransimitionMessage
        {
            public RpgfLocationInfo Location;

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
        #endregion

        public RpgfLocationInfo CurrentLocation { get; private set; } = null;
        public bool IsChanging => changingCoroutine != null;

        public event Action<RpgfLocationInfo> OnLocationChanged;

        private Coroutine changingCoroutine = null;

        #region API

        public void ChangeLocation(RpgfLocationInfo location, string pointName = "default")
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
        public void ChangeLocation(RpgfLocationInfo location, Vector2 position, ViewDirection direction)
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
        public void ChangeLocation(TransimitionMessage message)
        {
            if (IsChanging) return;

            changingCoroutine = StartCoroutine(ChangeLocationCoroutine(message));
        }

        public RpgfLocationInfo LoadLocationInfoByFileName(string fileName)
        {
            return Resources.Load<RpgfLocationInfo>($"Locations/{fileName}");
        }
        public RpgfLocationInfo LoadLocationInfoByName(string locName)
        {
            RpgfLocationInfo[] locations = Resources.LoadAll<RpgfLocationInfo>($"Locations");

            return locations.First(i => i.Name == locName);
        }

        #endregion

        private IEnumerator ChangeLocationCoroutine(TransimitionMessage message)
        {
            Global.LoadingScreen.ShowBackground();

            yield return new WaitWhile(() => Global.LoadingScreen.IsBackgroundFading);

            if (CurrentLocation != null && message.Location.SceneName == SceneManager.GetActiveScene().name)
                Local.Location.GetLocationByInfo(CurrentLocation).OnLeave();

            if (message.Location.SceneName != SceneManager.GetActiveScene().name)
            {
                Global.LoadingScreen.ShowProggresBar();

                Global.SceneLoader.LoadScene(message.Location.SceneName);

                yield return new WaitWhile(() => Global.SceneLoader.IsLoading);
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

            Global.LoadingScreen.HideBackground();
            Global.LoadingScreen.HideProggresBar();

            yield return new WaitWhile(() => Global.LoadingScreen.IsBackgroundFading);

            changingCoroutine = null;
        }
    }
}