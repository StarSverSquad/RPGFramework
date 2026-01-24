using DG.Tweening;
using RPGF.Domain;
using RPGF.Domain.Attributes;
using RPGF.EventSystem;
using RPGF.Explorer;
using System;
using UnityEngine;

namespace RPGF.Core.Character
{
    public abstract class CharacterModelControllerBase : RPGFrameworkBehaviour, IDisposable, ICharacterModelController
    {
        [Header("Базовые настройки:")]
        [DisplayName("Базовое направление")]
        [SerializeField]
        private ViewDirection _startDiration = ViewDirection.Down;
        [DisplayName("Авто позиция Z")]
        [SerializeField]
        private bool _autoOrdeting = true;
        [DisplayName("Авто инициализация")]
        [SerializeField]
        private bool _autoInitialize = false;

        [Header("Настройки под событие:")]
        [DisplayName("Связанное событие")]
        [SerializeField]
        private LocationEvent _linkedEvent;

        #region PROPS

        public ViewDirection Direction { get; private set; }

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

        public event Action<ViewDirection> OnRotateEvent;

        #endregion

        public override void Initialize()
        {
            RotateTo(_startDiration);

            if (_linkedEvent != null)
            {
                _linkedEvent.InnerEvent.OnStart += OnLinkedEventStart;
                _linkedEvent.InnerEvent.OnEnd += OnLinkedEventEnd;
            }
        }

        private void OnEnable()
        {
            if (_autoInitialize)
                Initialize();
        }

        private void Update()
        {
            if (_autoOrdeting)
                transform.position = new Vector3(transform.position.x, 
                                                 transform.position.y, 
                                                 transform.position.y);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #region MAIN API

        public void RotateTo(ViewDirection direction)
        {
            Direction = direction;

            OnRotateEvent?.Invoke(direction);
            OnRotate(direction);
        }
        
        public void MoveTo(Vector2 position, float time)
        {
            Vector2 vectorDiretion = (position - (Vector2)transform.position).normalized;
            ViewDirection viewDiretion = DirectionHelper.GetViewDirectionByVector(vectorDiretion);

            RotateTo(viewDiretion);

            MoveToNotRotate(position, time);
        }
        public void MoveToRelative(Vector2 offset, float time)
        {
            MoveTo((Vector2)transform.position + offset, time);
        }

        #endregion

        #region ADDATIVE API

        public void MoveToNotRotate(Vector2 position, float time)
        {
            DisposeMoveTween();

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
        public void MoveToRelativeNotRotate(Vector2 offset, float time)
        {
            MoveToNotRotate((Vector2)transform.position + offset, time);
        }

        public void RotateToPlayer()
        {
            Vector2 vectorDirection = (ExplorerManager.GetPlayerPosition3D() - transform.position).normalized;

            ViewDirection direction = DirectionHelper.GetViewDirectionByVector(vectorDirection);

            RotateTo(direction);
        }
        public void RotateToDefault()
        {
            RotateTo(_startDiration);
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
        public void StopMove()
        {
            DisposeMoveTween();

            MoveIsPaused = false;

            OnEndMove();
            OnEndMoveEvent?.Invoke();
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
            RotateToPlayer();

            OnLinkedEventStated?.Invoke();
        }
        protected virtual void OnLinkedEventEnd()
        {
            RotateToDefault();

            OnLinkedEventEnded?.Invoke();
        }

        protected virtual void OnRotate(ViewDirection direction) { }

        #endregion
    }
}
