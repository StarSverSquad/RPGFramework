using System;
using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Enums;
using UnityEngine;

namespace RPGF.Overworld.Player
{
    public class PlayerOverworldMovement : RPGFrameworkBehaviour
    {
        public bool CanWalk = true;
        public bool CanRun = true;
        public bool CanRotate = true;

        public float Speed = 10f;
        public float AccelerationFactor = 1.5f;

        public Vector2 Velocity = Vector2.zero;
        public Vector2 NormolizedVelocity = Vector2.zero;
        public Vector2 ExternalVelocity = Vector2.zero;

        public MoveDirection MoveDirection = MoveDirection.Stay;
        public ViewDirection ViewDirection = ViewDirection.Down;

        [SerializeField]
        private Rigidbody2D rb;

        #region PROPS

        public float ResultSpeed => !IsRun ? Speed : Speed * AccelerationFactor;
        public bool IsMoving { get; private set; }
        public bool IsRun { get; private set; }
        public bool IsAutoMoving => autoMoveTween != null;

        #endregion

        #region EVENTS
        public event Action OnMoving;
        public event Action OnStopMoving;
        public event Action OnStartMoving;
        public event Action OnStartRun;
        public event Action OnStopRun;
        public event Action<ViewDirection> OnRotate;
        #endregion

        private Tween autoMoveTween = null;

        private void Update()
        {
            if (Local == null)
                return;

            if (IsAutoMoving)
            {
                OnMoving?.Invoke();
            }
            else
            {
                Move();
            }
        }

        public void SetMovementAccess(bool active)
        {
            CanRun = active;
            CanWalk = active;
            CanRotate = active;
        }

        public void TranslateBySpeed(Vector2 vec, float speed)
        {
            TranslateByTime(vec, vec.magnitude / speed);
        }

        public void TranslateByTime(Vector2 vec, float time, Ease ease = Ease.Linear, Action onComplete = null)
        {
            DisposeAutoMoveTween();

            rb.linearVelocity = Vector2.zero;

            ViewDirection moveDir = DirectionHelper.GetViewDirectionByVector(vec.normalized);

            RotateTo(moveDir);

            autoMoveTween = transform.DOMove(vec, time).SetRelative().SetEase(ease);

            autoMoveTween.onPlay += () =>
            {
                OnStartMoving?.Invoke();
            };

            autoMoveTween.onComplete += () =>
            {
                OnStopMoving?.Invoke();
                DisposeAutoMoveTween();
                onComplete?.Invoke();
            };

            autoMoveTween.Play();
        }

        public void TranslateByParabola(Vector2 offset, float time, float arcHeight, Ease ease = Ease.Linear, Action onComplete = null)
        {
            DisposeAutoMoveTween();

            rb.linearVelocity = Vector2.zero;

            if (offset.sqrMagnitude > 0f)
            {
                ViewDirection moveDir = DirectionHelper.GetViewDirectionByVector(offset.normalized);
                RotateTo(moveDir);
            }

            Vector2 start = transform.position;
            Vector2 end = start + offset;
            Vector2 travelDirection = offset.sqrMagnitude > 0f ? offset.normalized : Vector2.right;
            Vector2 perpendicular = new(-travelDirection.y, travelDirection.x);
            Vector2 control = (start + end) * 0.5f + perpendicular * arcHeight;

            autoMoveTween = DOVirtual.Float(0f, 1f, time, t =>
            {
                float invertedT = 1f - t;
                Vector2 position = invertedT * invertedT * start + 2f * invertedT * t * control + t * t * end;
                transform.position = position;
                OnMoving?.Invoke();
            }).SetEase(ease);

            autoMoveTween.onPlay += () =>
            {
                OnStartMoving?.Invoke();
            };

            autoMoveTween.onComplete += () =>
            {
                transform.position = end;
                OnStopMoving?.Invoke();
                DisposeAutoMoveTween();
                onComplete?.Invoke();
            };

            autoMoveTween.Play();
        }

        public void SnapTo(Vector2 position)
        {
            DisposeAutoMoveTween();

            rb.linearVelocity = Vector2.zero;
            transform.position = position;

            OnMoving?.Invoke();
            OnStopMoving?.Invoke();
        }

        public void RotateTo(ViewDirection direction)
        {
            ViewDirection = direction;

            OnRotate?.Invoke(direction);
        }

        private void Move()
        {
            Velocity = Vector2.zero;

            MoveDirection = MoveDirection.Stay;
            NormolizedVelocity = Vector2.zero;

            ViewDirection? newViewDirection = null;

            if (CanWalk && !Overworld.EventHandler.EventPlaying)
            {

                if (Input.GetKey(Global.BaseOptions.MoveRight))
                {
                    MoveDirection = MoveDirection.Right;
                    newViewDirection = ViewDirection.Right;
                    Velocity += new Vector2(1, 0);
                }

                if (Input.GetKey(Global.BaseOptions.MoveLeft))
                {
                    MoveDirection = MoveDirection.Left;
                    newViewDirection = ViewDirection.Left;
                    Velocity += new Vector2(-1, 0);
                }

                if (Input.GetKey(Global.BaseOptions.MoveUp))
                {
                    MoveDirection = MoveDirection.Up;
                    newViewDirection = ViewDirection.Up;
                    Velocity += new Vector2(0, 1);
                }

                if (Input.GetKey(Global.BaseOptions.MoveDown))
                {
                    MoveDirection = MoveDirection.Down;
                    newViewDirection = ViewDirection.Down;
                    Velocity += new Vector2(0, -1);
                }
            }

            if (Velocity.magnitude > 0)
            {
                if (!IsMoving)
                    OnStartMoving?.Invoke();

                IsMoving = true;

                OnMoving?.Invoke();
            }
            else
            {
                IsMoving = false;

                OnStopMoving?.Invoke();

                if (ExternalVelocity.sqrMagnitude > 0f)
                    OnMoving?.Invoke();
            }


            if (CanRotate && newViewDirection.HasValue)
            {
                if (ViewDirection != newViewDirection)
                {
                    ViewDirection = newViewDirection.Value;
                    OnRotate?.Invoke(newViewDirection.Value);
                }
            }

            NormolizedVelocity = Velocity.normalized;

            bool nRun = Input.GetKey(Global.BaseOptions.Run) && CanRun;

            if (nRun && !IsRun)
                OnStartRun?.Invoke();
            else if (!nRun && IsRun)
                OnStopRun?.Invoke();

            IsRun = nRun;

            Velocity = ResultSpeed * Velocity.normalized;

            rb.linearVelocity = Velocity + ExternalVelocity;
        }

        private void DisposeAutoMoveTween()
        {
            if (autoMoveTween != null)
            {
                autoMoveTween.Kill();
                autoMoveTween = null;
            }
        }
    }
}