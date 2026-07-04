using NaughtyAttributes;
using RPGF.Core;
using RPGF.GUI.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPGF.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class GUIInteractable : RPGFrameworkBehaviour, IGUIInteractable
    {
        public bool Focused { get; private set; }
        public bool Selected { get; private set; }

        private RectTransform rectTransform;
        public RectTransform RectTransform => rectTransform;

        [Foldout("Events")]
        public UnityEvent OnFocus;
        [Foldout("Events")]
        public UnityEvent OnUnfocus;
        [Foldout("Events")]
        public UnityEvent OnCancel;
        [Foldout("Events")]
        public UnityEvent OnSelect;

        public override void Initialize()
        {
            base.Initialize();

            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void Cancel()
        {
            if (!gameObject.activeInHierarchy)
                return;

            OnCancel?.Invoke();
            OnCanceled();
        }

        public virtual void Select()
        {
            if (!gameObject.activeInHierarchy)
                return;

            OnSelect?.Invoke();
            OnSelected();
        }

        public virtual void SetFocus(bool focus)
        {
            if (!gameObject.activeInHierarchy)
                return;

            Focused = focus;

            if (focus)
            {
                OnFocus?.Invoke();
                OnFocused();
            }
            else
            {
                OnUnfocus?.Invoke();
                OnUnfocused();
            }
                
        }

        #region VIRTUALS 

        public virtual void OnCanceled() { }
        public virtual void OnSelected() { }
        public virtual void OnFocused() { }
        public virtual void OnUnfocused() { }

        #endregion

        public virtual void Dispose() { }
    }
}
