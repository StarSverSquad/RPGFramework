using RPGF.Core;
using RPGF.GUI.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI.Abstractions
{
    public abstract class GUIBlockBase : RPGFrameworkBehaviour, IGUIBlock
    {
        [SerializeField]
        private bool _disableOnDiactivate = true;
        public bool DisableOnDiactivate => _disableOnDiactivate;

        [SerializeField]
        private bool _enableOnActivate = true;
        public bool EnableOnActivate => _enableOnActivate;

        public IGUIManager Manager { get; protected set; }

        public bool IsActivated { get; protected set; } = false;
        public bool IsFocused { get; protected set; } = false;

        #region EVENTS

        [Space]
        public UnityEvent OnActivateEvent;
        public UnityEvent OnDiativateEvent;
        [Space]
        public UnityEvent OnDisposeEvent;
        [Space]
        public UnityEvent OnFocusEvent;
        public UnityEvent OnLostFocusEvent;
        [Space]
        public UnityEvent OnPreviewEvent;
        public UnityEvent<GUIBlockBase> OnNextEvent;

        #endregion

        /// <remarks>[НЕ РЕКОМЕНДУЕТСЯ]</remarks>
        public override void Initialize()
        {
            Debug.LogWarning("Возможны ошибки!");
        }
        public virtual void Initialize(IGUIManager manager)
        {
            Manager = manager;
        }

        #region MAIN API

        public void Activate()
        {
            if (IsActivated)
                return;

            if (_enableOnActivate)
                gameObject.SetActive(true);

            if (gameObject.activeInHierarchy)
            {
                OnActivate();
                OnActivateEvent?.Invoke();
            }      

            IsActivated = true;
        }
        public void Diativate()
        {
            if (!IsActivated)
                return;

            if (gameObject.activeInHierarchy)
            {
                OnDiativate();
                OnDiativateEvent?.Invoke();
            }

            if (_disableOnDiactivate)
                gameObject.SetActive(false);

            IsActivated = false;
        }

        public virtual void Dispose()
        {
            if (gameObject.activeInHierarchy)
            {
                OnDispose();
                OnDisposeEvent?.Invoke();
            }
        }

        public virtual void SetFocus(bool focus)
        {
            if (!gameObject.activeInHierarchy)
            {
                Debug.LogWarning("Can't set focus on deactivated block");
                return;
            }

            IsFocused = focus;

            if (focus)
            {
                OnFocus();
                OnFocusEvent?.Invoke();
            }
            else
            {
                OnLostFocus();
                OnLostFocusEvent?.Invoke();
            }
        }

        public virtual void Preview()
        {
            OnPreview();
            OnPreviewEvent?.Invoke();

            Dispose();

            Manager.PreviewBlock();
        }
        public virtual void Next(IGUIBlock gUIBlock)
        {
            var block = (GUIBlockBase)gUIBlock;

            OnNext(block);
            OnNextEvent?.Invoke(block);

            Manager.NextBlock(block);
        }
        public void Next(GUIBlockBase gUIBlock)
        {
            Next(gUIBlock as IGUIBlock);
        }

        #endregion

        #region VIRTUALS
        protected virtual void OnActivate() { }
        protected virtual void OnDiativate() { }

        protected virtual void OnFocus() { }
        protected virtual void OnLostFocus() { }

        protected virtual void OnDispose() { }

        protected virtual void OnPreview() { }
        protected virtual void OnNext(GUIBlockBase gUIBlock) { }

        #endregion

        protected virtual void OnDisable()
        {
            Dispose();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}
