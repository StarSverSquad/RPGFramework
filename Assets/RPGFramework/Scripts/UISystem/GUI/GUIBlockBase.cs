using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    public abstract class GUIBlockBase : RPGFrameworkBehaviour, IGUIBlock
    {
        public GUIManagerBase Manager { get; private set; }

        public bool IsActivated { get; private set; }

        public UnityEvent OnActivateEvent;
        public UnityEvent OnDiativateEvent;
        public UnityEvent OnDisposeEvent;
        public UnityEvent OnPreviewEvent;
        public UnityEvent<GUIBlockBase> OnNextEvent;

        public override void Initialize()
        {
            Debug.LogWarning("НЕ РЕКОМЕНДУЕТСЯ");
        }
        public virtual void Initialize(GUIManagerBase manager)
        {
            Manager = manager;

            IsActivated = false;
        }

        #region MAIN API

        public void Activate()
        {
            OnActivate();
            OnActivateEvent?.Invoke();

            IsActivated = true;
        }
        public void Diativate()
        {
            OnDiativate();
            OnDiativateEvent?.Invoke();

            IsActivated = false;
        }
        public void Dispose()
        {
            Diativate();

            OnDispose();
            OnDisposeEvent?.Invoke();
        }

        public void Preview()
        {
            OnPreview();
            OnPreviewEvent?.Invoke();

            Dispose();

            Manager.PreviewBlock();
        }
        public void Next(IGUIBlock gUIBlock)
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

        protected virtual void OnActivate() { }
        protected virtual void OnDiativate() { }
        protected virtual void OnDispose() { }

        protected virtual void OnPreview() { }
        protected virtual void OnNext(GUIBlockBase gUIBlock) { }
    }
}
