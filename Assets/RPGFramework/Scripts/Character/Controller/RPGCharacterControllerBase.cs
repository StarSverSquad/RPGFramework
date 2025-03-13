using DG.Tweening;
using System;
using UnityEngine;

namespace RPGF.Character
{
    public abstract class RPGCharacterControllerBase : RPGFrameworkBehaviour, IDisposable, IRPGCharacterController
    {
        [SerializeField]
        private Direction _startDiration;

        #region PROPS

        public Direction Direction { get; private set; }

        public bool IsMove => moveTween != null;

        #endregion

        #region PRIVATE/PROTECTED FIELDS

        private Tween moveTween = null;

        #endregion

        #region EVENTS

        public event Action OnStartMoveEvent;
        public event Action OnEndMoveEvent;

        public event Action<Direction> OnRotateEvent;

        #endregion

        public override void Initialize()
        {
            RotateTo(Direction);
        }

        #region MAIN API

        public void RotateTo(Direction direction)
        {
            Direction = direction;

            OnRotateEvent?.Invoke(direction);
            OnRotate(direction);
        }

        public void MoveTo(Vector2 position, float time)
        {
            DisposeMoveTween();

            Vector2 vectorDiretion = (Vector2)transform.position - position;

            Direction moveDiretion = GetDirectionByVector(vectorDiretion);

            moveTween = transform.DOMove(position, time).Play();

            moveTween.onComplete += () =>
            {
                OnEndMove();
                OnEndMoveEvent?.Invoke();

                moveTween = null;
            };
            moveTween.onPlay += () =>
            {
                OnStartMove();
                OnStartMoveEvent?.Invoke();
            };
        }

        public void MoveToRelative(Vector2 offset, float time)
        {
            MoveTo((Vector2)transform.position + offset, time);
        }

        #endregion

        #region ADDATIVE API

        protected Direction GetDirectionByVector(Vector2 vector)
        {
            if (vector.y > 0.20f)
                return Direction.Up;
            else if (vector.y < 0.20f)
                return Direction.Down;
            else if (vector.x > 0)
                return Direction.Right;
            else if (vector.x < 0)
                return Direction.Left;
            else
                return Direction.Down;
        }

        protected void DisposeMoveTween()
        {
            if (moveTween != null)
            {
                moveTween.Kill();
                moveTween = null;
            }
        }

        #endregion

        public virtual void Dispose()
        {
            DisposeMoveTween();
        }

        #region VIRTUALS

        protected virtual void OnStartMove() { }
        protected virtual void OnEndMove() { }
        protected virtual void OnRotate(Direction direction) { }

        #endregion
    }
}
