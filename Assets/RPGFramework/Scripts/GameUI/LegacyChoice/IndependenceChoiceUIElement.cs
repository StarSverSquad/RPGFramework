using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IndependenceChoiceUIElement : MonoBehaviour
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

    public Vector2 Margin;

    public UnityEvent OnFocus;
    public UnityEvent OnLostFocus;
    public UnityEvent OnLocked;
    public UnityEvent OnUnlocked;

    public void Initialize(string text, Sprite iconSprite = null, string counter = "")
    {
        Vector4 m = new Vector4(Margin.x, 0, Margin.y, 0);

        if (iconSprite != null)
        {
            icon.sprite = iconSprite;
            icon.gameObject.SetActive(true);
            m.x += icon.rectTransform.sizeDelta.x;
        }
        else
            icon.gameObject.SetActive(false);

        if (counter != "")
        {
            counterText.gameObject.SetActive(true);
            counterText.text = counter;
            m.z += counterText.GetPreferredValues().x;
        }
        else
            counterText.gameObject.SetActive(false);
        
        mainText.text = text;

        mainText.margin = m;
    }

    public void SetFocus(bool focus)
    {
        IsFocused = focus;

        if (focus)
            OnFocus?.Invoke();
        else
            OnLostFocus?.Invoke();
    }

    public void SetLock(bool locked)
    {
        IsLocked = locked;

        if (locked)
            OnLocked?.Invoke();
        else
            OnUnlocked?.Invoke();
    }
}