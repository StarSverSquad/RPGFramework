using System;
using DG.Tweening;
using RPGF.Core.Enums;
using RPGF.EventSystem;
using RPGF.Explorer;
using UnityEngine;

namespace RPGF.Core.Character
{
    public abstract class CharacterModelControllerBase : RPGFrameworkBehaviour, IDisposable, ICharacterModelController
    {
        [Header("Starting direction:")]
        [SerializeField]
        private ViewDirection startDirection = ViewDirection.Down;
        [Header("Config:")]
        [SerializeField]
        private bool autoOrdering = true;
        [SerializeField]
        private bool autoInitialize = false;
        [SerializeField]
        private string talkTag = string.Empty;

        [Header("Linked event:")]
        [SerializeField]
        private LocationEvent linkedEvent;

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
            RotateTo(startDirection);

            if (linkedEvent != null)
            {
                linkedEvent.InnerEvent.OnStart += OnLinkedEventStart;
                linkedEvent.InnerEvent.OnEnd += OnLinkedEventEnd;
            }
        }

        private void OnEnable()
        {
            if (autoInitialize)
                Initialize();
        }

        private void Update()
        {
            if (autoOrdering)
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
            Vector2 vectorDirection = (position - (Vector2)transform.position).normalized;
            ViewDirection viewDirection = DirectionHelper.GetViewDirectionByVector(vectorDirection);

            RotateTo(viewDirection);

            MoveToNotRotate(position, time);
        }
        public void MoveToRelative(Vector2 offset, float time)
        {
            MoveTo((Vector2)transform.position + offset, time);
        }

        public void Talk()
        {
            throw new NotImplementedException();
        }

        public void StopTalk()
        {
            throw new NotImplementedException();
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
            RotateTo(startDirection);
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
            moveTween?.Kill();
            moveTween = null;
        }

        #endregion

        public virtual void Dispose()
        {
            DisposeMoveTween();

            if (linkedEvent != null)
            {
                linkedEvent.InnerEvent.OnStart -= OnLinkedEventStart;
                linkedEvent.InnerEvent.OnEnd -= OnLinkedEventEnd;
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
