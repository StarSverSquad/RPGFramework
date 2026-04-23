using NaughtyAttributes;
using RPGF.Core;
using RPGF.GUI.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class GUIElement : RPGFrameworkBehaviour, IGUIElement
    {
        private RectTransform rectTransform;
        public RectTransform RectTransform => rectTransform;

        public bool Focused { get; protected set; }

        [Foldout("Element events")]
        public UnityEvent OnFocus;
        [Foldout("Element events")]
        public UnityEvent OnUnfocus;

        public override void Initialize()
        {
            base.Initialize();

            rectTransform = GetComponent<RectTransform>();
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
                OnLostFocus();
            }
        }

        public virtual void OnFocused() { }
        public virtual void OnLostFocus() { }

        public virtual void Dispose() { }
    }
}
