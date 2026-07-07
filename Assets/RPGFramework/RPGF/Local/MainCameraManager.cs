using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using System;
using RPGF.Core;
using RPGF.Explorer;
using RPGF.Core.Location;

namespace RPGF
{
    public class MainCameraManager : RPGFrameworkBehaviour
    {
        public enum CaptureType
        {
            Player, PlayerFollow, LocationPoint, FreeMove, None
        }

        public float ZPosition = -5000;

        [FormerlySerializedAs("PlayerFollowBorder")]
        [SerializeField] private Vector2 defaultPlayerFollowBorder = new(5f, 4f);

        private Vector2 playerFollowBorder;
        private Vector2 playerFollowAnchor;

        #region PROPS

        private CaptureType capture;
        public CaptureType Capture
        {
            get => capture;

            private set
            {
                capture = value;
                OnCaptureChanged?.Invoke(value);
            }
        }

        #endregion

        private Tween moveTween = null;

        #region EVENTS

        public Action OnFreeMoveEnd;
        public Action OnFreeMoveStart;

        public Action<CaptureType> OnCaptureChanged;

        #endregion

        public override void Initialize()
        {
            capture = CaptureType.None;
        }

        private void Update()
        {
            if (Capture == CaptureType.PlayerFollow)
                PlayerFollow();
        }

        #region API

        public void PlaceToLocationPoint(LocationController obj = null)
        {
            Capture = CaptureType.LocationPoint;

            LocationController loc = obj ?? LocalManager.GetCurrentLocation();

            transform.position = new Vector3(
                loc.CameraPoint.position.x,
                loc.CameraPoint.position.y,
                ZPosition);
        }

        public void FollowToPlayer(Vector2 anchor, Vector2 border)
        {
            playerFollowAnchor = anchor;
            playerFollowBorder = border != Vector2.zero ? border : defaultPlayerFollowBorder;
            Capture = CaptureType.PlayerFollow;
        }

        public void PlaceToPlayer()
        {
            Capture = CaptureType.Player;
            Vector2 playerPos = ExplorerManager.GetPlayerPosition();
            transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);

        }

        public void MoveTo(Vector2 position, float time, Ease easing = Ease.Linear)
        {
            DisposeMoveTween();

            Capture = CaptureType.FreeMove;

            moveTween = transform.DOMove(position, time).SetEase(easing);

            moveTween.onPlay += () =>
            {
                OnFreeMoveStart?.Invoke();
            };

            moveTween.onComplete += () =>
            {
                Capture = CaptureType.None;

                OnFreeMoveEnd?.Invoke();

                DisposeMoveTween();
            };

            moveTween.Play();
        }

        #endregion

        private void PlayerFollow()
        {
            Vector2 playerPosition = ExplorerManager.GetPlayerPosition();
            Vector2 cameraPosition = transform.position;
            Vector2 cameraToPlayer = playerPosition - cameraPosition;

            Vector2 borderX = new(
                cameraPosition.x + playerFollowBorder.x,
                cameraPosition.x - playerFollowBorder.x
                );

            Vector2 borderY = new(
                cameraPosition.y + playerFollowBorder.y,
                cameraPosition.y - playerFollowBorder.y
                );

            Vector3 newCameraPosition = Vector3.zero;

            newCameraPosition.z = ZPosition;
            newCameraPosition.x = cameraPosition.x;
            newCameraPosition.y = cameraPosition.y;

            if (playerPosition.x > borderX.x || playerPosition.x < borderX.y)
            {
                bool minus = cameraToPlayer.x < 0;

                float absDistance = Mathf.Abs(cameraToPlayer.x) - playerFollowBorder.x;

                newCameraPosition.x += minus ? -absDistance : absDistance;
            }

            if (playerPosition.y > borderY.x || playerPosition.y < borderY.y)
            {
                bool minus = cameraToPlayer.y < 0;

                float absDistance = Mathf.Abs(cameraToPlayer.y) - playerFollowBorder.y;

                newCameraPosition.y += minus ? -absDistance : absDistance;
            }

            newCameraPosition.x = Mathf.Clamp(
                newCameraPosition.x,
                playerFollowAnchor.x - playerFollowBorder.x,
                playerFollowAnchor.x + playerFollowBorder.x);
            newCameraPosition.y = Mathf.Clamp(
                newCameraPosition.y,
                playerFollowAnchor.y - playerFollowBorder.y,
                playerFollowAnchor.y + playerFollowBorder.y);

            transform.position = newCameraPosition;
        }

        private void DisposeMoveTween()
        {
            if (moveTween != null)
            {
                moveTween.Kill();
                moveTween = null;
            }
        }
    }
}