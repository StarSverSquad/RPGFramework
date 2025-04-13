using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    public class GUIElementBase : RPGFrameworkBehaviour, IGUIElement
    {
        public bool Focused { get; private set; }
        public bool Selected { get; private set; }

        public UnityEvent OnFocus;
        public UnityEvent OnUnfocus;
        public UnityEvent OnCancel;
        public UnityEvent OnSelect;

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
            {
                Debug.LogWarning("Can't set focus on deactivated element");
                return;
            }

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
    }
}
