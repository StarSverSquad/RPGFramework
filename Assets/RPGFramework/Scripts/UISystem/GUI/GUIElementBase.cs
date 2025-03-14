using System;
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
            OnCancel?.Invoke();
        }

        public virtual void Select()
        {
            OnSelect?.Invoke();
        }

        public virtual void SetFocus(bool focus)
        {
            Focused = focus;

            if (focus)
                OnFocus?.Invoke();
            else
                OnUnfocus?.Invoke();
        }
    }
}
