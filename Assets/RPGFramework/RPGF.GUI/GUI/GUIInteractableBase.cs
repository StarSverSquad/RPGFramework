using RPGF.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPGF.GUI
{
    [RequireComponent(typeof(RectTransform))]
    public class GUIInteractableBase : RPGFrameworkBehaviour, IGUIInteractable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private bool withMouseInteract = true;
        public bool WithMouseInteract => withMouseInteract;

        public bool Focused { get; private set; }
        public bool Selected { get; private set; }

        private RectTransform rectTransform;
        public RectTransform RectTransform => rectTransform;

        public UnityEvent OnFocus;
        public UnityEvent OnUnfocus;
        public UnityEvent OnCancel;
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

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (WithMouseInteract)
                SetFocus(true);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (withMouseInteract)
                SetFocus(false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (WithMouseInteract && eventData.button == PointerEventData.InputButton.Left)
                Select();
        }

        #endregion

        public virtual void Dispose() { }
    }
}
