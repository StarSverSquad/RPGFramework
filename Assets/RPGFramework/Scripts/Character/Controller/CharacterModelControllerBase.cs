using DG.Tweening;
using System;
using UnityEngine;

namespace RPGF.Character
{
    public abstract class CharacterModelControllerBase : RPGFrameworkBehaviour, IDisposable, ICharacterModelController
    {
        [Header("Базовые настройки:")]
        [SerializeField]
        private Direction _startDiration = Direction.Down;
        [SerializeField]
        private bool _autoOrdeting = true;

        [Header("Настройки под событие:")]
        [SerializeField]
        private LocationEvent _linkedEvent;

        #region PROPS

        public Direction Direction { get; private set; }

        public bool IsMove => moveTween != null;
        public bool MoveIsPaused { get; private set; } = false;

        #endregion

        #region PRIVATE/PROTECTED FIELDS

        private Tween moveTween = null;

        #endregion

        #region EVENTS

        public event Action OnStartMoveEvent;
        public event Action OnEndMoveEvent;
        public event Action OnPauseMoveEvent;
        public event Action OnResumeMoveEvent;

        public event Action OnLinkedEventStated;
        public event Action OnLinkedEventEnded;

        public event Action<Direction> OnRotateEvent;

        #endregion

        public override void Initialize()
        {
            RotateTo(Direction);

            if (_linkedEvent != null)
            {
                _linkedEvent.InnerEvent.OnStart += OnLinkedEventStart;
                _linkedEvent.InnerEvent.OnEnd += OnLinkedEventEnd;
            }
        }

        private void Update()
        {
            if (_autoOrdeting)
                transform.position = new Vector3(transform.position.x, 
                                                 transform.position.y, 
                                                 -transform.position.y);
        }

        private void OnDestroy()
        {
            Dispose();
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

            moveTween = transform.DOMove(position, time).SetEase(Ease.Linear).Play();

            MoveIsPaused = false;

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

        public void RotateToPlayer()
        {
            Vector2 vectorDirection = transform.position - ExplorerManager.GetPlayerPosition3D();

            Direction direction = GetDirectionByVector(vectorDirection);

            RotateTo(direction);
        }

        public void PauseMove()
        {
            moveTween?.Pause();

            MoveIsPaused = true;

            OnPauseMove();
            OnPauseMoveEvent?.Invoke();
        }
        public void ResumeMove()
        {
            moveTween?.Play();

            MoveIsPaused = true;

            OnResumeMove();
            OnResumeMoveEvent?.Invoke();
        }

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

            if (_linkedEvent != null)
            {
                _linkedEvent.InnerEvent.OnStart -= OnLinkedEventStart;
                _linkedEvent.InnerEvent.OnEnd -= OnLinkedEventEnd;
            }
        }

        #region VIRTUALS

        protected virtual void OnStartMove() { }
        protected virtual void OnEndMove() { }
        protected virtual void OnPauseMove() { }
        protected virtual void OnResumeMove() { }

        protected virtual void OnLinkedEventStart()
        {
            OnLinkedEventStated?.Invoke();
        }
        protected virtual void OnLinkedEventEnd()
        {
            OnLinkedEventEnded?.Invoke();
        }

        protected virtual void OnRotate(Direction direction) { }

        #endregion
    }
}
