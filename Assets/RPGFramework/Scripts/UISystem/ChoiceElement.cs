using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPGF.Choice
{
    public class ChoiceElement : MonoBehaviour
    {
        [SerializeField]

        protected TextMeshProUGUI mainText;
        [SerializeField]
        protected TextMeshProUGUI counterText;
        [SerializeField]
        protected Image icon;

        public RectTransform RectTransform => GetComponent<RectTransform>();

        public bool IsLocked { get; private set; } = false;
        public bool IsFocused { get; private set; } = false;

        public UnityEvent OnFocus;
        public UnityEvent OnLostFocus;
        public UnityEvent OnLocked;
        public UnityEvent OnUnlocked;
        public UnityEvent OnSelected;
        public UnityEvent OnFailSelect;

        public void Setup(string text, Sprite iconSprite = null, string counter = "")
        {
            Vector4 maintexMargin = Vector4.zero;

            if (iconSprite != null)
            {
                icon.sprite = iconSprite;
                icon.gameObject.SetActive(true);

                maintexMargin.x = 62;
            }
            else
            {
                icon.gameObject.SetActive(false);

                maintexMargin.x = 12;
            }

            if (!string.IsNullOrEmpty(counter))
            {
                counterText.gameObject.SetActive(true);
                counterText.text = counter;

                maintexMargin.z = 74;
            }
            else
            {
                counterText.gameObject.SetActive(false);
            }

            mainText.margin = maintexMargin;
            mainText.text = text;
        }

        public virtual void SetFocus(bool focus)
        {
            IsFocused = focus;

            if (IsLocked)
                OnLocked?.Invoke();
            else
                OnUnlocked?.Invoke();

            if (focus)
            {
                OnFocus?.Invoke();
            }
            else
            {
                OnLostFocus?.Invoke();
            }

        }

        public virtual void SetLock(bool locked)
        {
            IsLocked = locked;

            if (locked)
                OnLocked?.Invoke();
            else
                OnUnlocked?.Invoke();
        }   

        public virtual void Selected()
        {
            OnSelected?.Invoke();
        }

        public virtual void FailSelect()
        {
            OnFailSelect?.Invoke();
        }
    }
}