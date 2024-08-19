using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonChoiceUIElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI mainText;
    [SerializeField]
    private TextMeshProUGUI counterText;
    [SerializeField]
    private Image icon;

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

    public void SetFocus(bool focus)
    {
        IsFocused = focus;

        if (focus)
            OnFocus?.Invoke();
        else
        {
            //if (IsLocked)
            //    OnLocked?.Invoke();

            OnLostFocus?.Invoke();
        }

    }

    public void SetLock(bool locked)
    {
        IsLocked = locked;

        if (locked)
            OnLocked?.Invoke();
        else
            OnUnlocked?.Invoke();
    }   

    public void Selected()
    {
        OnSelected?.Invoke();
    }

    public void FailSelect()
    {
        OnFailSelect?.Invoke();
    }
}