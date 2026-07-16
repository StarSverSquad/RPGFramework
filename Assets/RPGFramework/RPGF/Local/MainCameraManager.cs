using System;
using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Location;
using RPGF.Overworld;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField]
        private Vector2 defaultPlayerFollowBorder = new(5f, 4f);
        [SerializeField]
        private Vector2 playerGap = new(3f, 3f);

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

            Vector2 playerPosition = OverworldManager.GetPlayerPosition();
            Vector2 clampedPosition = ClampCameraCenter(playerPosition);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, ZPosition);
        }

        public void PlaceToPlayer()
        {
            Capture = CaptureType.Player;
            Vector2 playerPos = OverworldManager.GetPlayerPosition();
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
            Vector2 playerPosition = OverworldManager.GetPlayerPosition();
            Vector2 cameraPosition = transform.position;
            Vector2 newCameraPosition = cameraPosition;

            if (TryGetCameraHalfExtents(out Vector2 halfExtents))
            {
                float rightEdge = cameraPosition.x + halfExtents.x;
                float leftEdge = cameraPosition.x - halfExtents.x;
                float topEdge = cameraPosition.y + halfExtents.y;
                float bottomEdge = cameraPosition.y - halfExtents.y;

                float followRight = rightEdge - playerGap.x;
                float followLeft = leftEdge + playerGap.x;
                float followTop = topEdge - playerGap.y;
                float followBottom = bottomEdge + playerGap.y;

                if (playerPosition.x > followRight)
                    newCameraPosition.x += playerPosition.x - followRight;
                else if (playerPosition.x < followLeft)
                    newCameraPosition.x += playerPosition.x - followLeft;

                if (playerPosition.y > followTop)
                    newCameraPosition.y += playerPosition.y - followTop;
                else if (playerPosition.y < followBottom)
                    newCameraPosition.y += playerPosition.y - followBottom;
            }

            Vector2 clampedPosition = ClampCameraCenter(newCameraPosition);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, ZPosition);
        }

        private bool TryGetCameraHalfExtents(out Vector2 halfExtents)
        {
            var camera = Camera.main;
            if (camera != null && camera.orthographic)
            {
                float halfHeight = camera.orthographicSize;
                halfExtents = new Vector2(halfHeight * camera.aspect, halfHeight);
                return true;
            }

            halfExtents = Vector2.zero;
            return false;
        }

        private Vector2 ClampCameraCenter(Vector2 center)
        {
            if (!TryGetCameraHalfExtents(out Vector2 halfExtents))
            {
                return new Vector2(
                    Mathf.Clamp(center.x, playerFollowAnchor.x - playerFollowBorder.x, playerFollowAnchor.x + playerFollowBorder.x),
                    Mathf.Clamp(center.y, playerFollowAnchor.y - playerFollowBorder.y, playerFollowAnchor.y + playerFollowBorder.y));
            }

            float minX = playerFollowAnchor.x - playerFollowBorder.x + halfExtents.x;
            float maxX = playerFollowAnchor.x + playerFollowBorder.x - halfExtents.x;
            float minY = playerFollowAnchor.y - playerFollowBorder.y + halfExtents.y;
            float maxY = playerFollowAnchor.y + playerFollowBorder.y - halfExtents.y;

            if (minX > maxX)
                minX = maxX = playerFollowAnchor.x;

            if (minY > maxY)
                minY = maxY = playerFollowAnchor.y;

            return new Vector2(
                Mathf.Clamp(center.x, minX, maxX),
                Mathf.Clamp(center.y, minY, maxY));
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