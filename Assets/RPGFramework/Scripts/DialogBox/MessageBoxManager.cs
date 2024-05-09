using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxManager : TextWriterBase
{
    public enum DialogBoxPosition
    {
        Bottom, Center, Top
    }

    [Header("Message")]
    [Tooltip("0 - bottom, 1 - center, 2 - top")]
    [SerializeField]
    private RectTransform[] messageBoxYPoints = new RectTransform[3];

    [Tooltip("0 - bottom, 1 - center, 2 - top")]
    [SerializeField]
    private RectTransform[] nameBoxYPoints = new RectTransform[3];

    [Tooltip("0 - without image, 1 - with image")]
    [SerializeField]
    private float[] textMargins = new float[2];

    [SerializeField]
    private RectTransform messageBox;
    [SerializeField]
    private RectTransform nameBox;

    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private AudioSource letterEffect;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI textMeshProNameBox;

    private TextVisualEffectBase textEffect = null;

    public MessageInfo Message => message is MessageInfo m ? m : null;

    public override void Initialize()
    {
        base.Initialize();

        arrow.SetActive(false);
        messageBox.gameObject.SetActive(false);
        nameBox.gameObject.SetActive(false);
    }

    public void Write(MessageInfo message)
    {
        base.InvokeWrite(message);
    }

    private void SetupDialog()
    {
        switch (Message.position)
        {
            case DialogBoxPosition.Bottom:
                messageBox.anchoredPosition = new Vector2(messageBox.anchoredPosition.x, messageBoxYPoints[0].anchoredPosition.y);
                nameBox.anchoredPosition = new Vector2(nameBox.anchoredPosition.x, nameBoxYPoints[0].anchoredPosition.y);
                break;
            case DialogBoxPosition.Center:
                messageBox.anchoredPosition = new Vector2(messageBox.anchoredPosition.x, messageBoxYPoints[1].anchoredPosition.y);
                nameBox.anchoredPosition = new Vector2(nameBox.anchoredPosition.x, nameBoxYPoints[1].anchoredPosition.y);
                break;
            case DialogBoxPosition.Top:
                messageBox.anchoredPosition = new Vector2(messageBox.anchoredPosition.x, messageBoxYPoints[2].anchoredPosition.y);
                nameBox.anchoredPosition = new Vector2(nameBox.anchoredPosition.x, nameBoxYPoints[2].anchoredPosition.y);
                break;
        }

        if (Message.name != string.Empty)
        {
            textMeshProNameBox.text = Message.name;

            nameBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textMeshProNameBox.GetPreferredValues().x + textMeshProNameBox.margin.x);

            nameBox.gameObject.SetActive(true);
        }
        else
            nameBox.gameObject.SetActive(false);

        if (Message.image != null)
        {
            image.sprite = Message.image;

            textMeshPro.margin = new Vector4(textMargins[1], textMeshPro.margin.y,
                                            textMeshPro.margin.z, textMeshPro.margin.w);

            image.gameObject.SetActive(true);
        }
        else
        {
            textMeshPro.margin = new Vector4(textMargins[0], textMeshPro.margin.y,
                                            textMeshPro.margin.z, textMeshPro.margin.w);

            image.gameObject.SetActive(false);
        }

        messageBox.gameObject.SetActive(true);
    }

    /// <summary>
    /// [Не рекомендуется] используйте метод Write
    /// </summary>
    public override void InvokeWrite(WriterMessage message)
    {
        if (message is not MessageInfo)
        {
            Debug.LogError("Не тот тип: либо используйте DialogMessage, либо метод Write");

            return;
        }

        base.InvokeWrite(message);
    }

    public override bool ContinueCanExecute()
    {
        return Input.GetKeyDown(GameManager.Instance.BaseOptions.Accept);
    }

    public override bool SkipCanExecute()
    {
        return Input.GetKeyDown(GameManager.Instance.BaseOptions.Cancel);
    }

    public override void OnEveryLetter(char letter)
    {
        if (Message.letterSound != null)
        {
            letterEffect.Play();
        }

        // Эфекты текста нужно наверное даработать
        if (textEffect != null)
        {
            textEffect.EndLetter = textMeshPro.text.Length;
        }
    }

    public override void OnStartWriting()
    {
        letterEffect.clip = Message.letterSound;

        // Эфекты текста нужно наверное даработать
        if (Message.textEffectTypeName != "None" && Message.textEffectTypeName != string.Empty)
        {
            TextVisualEffectBase effect = (TextVisualEffectBase)Activator.CreateInstance(GetType().Assembly.GetType(Message.textEffectTypeName), new object[] { textMeshPro, this });

            effect.StartLetter = 0;

            effect.StartEffect();

            textEffect = effect;
        }

        SetupDialog();
    }

    public override void OnEndWriting()
    {
        if (Message.closeWindow)
        {
            messageBox.gameObject.SetActive(false);
            nameBox.gameObject.SetActive(false);
        }

        if (textEffect != null)
        {
            textEffect.StopEffect();

            textEffect = null;
        }
    }

    public override void OnWait()
    {
        arrow.SetActive(true);
    }

    public override void OnEndWait()
    {
        arrow.SetActive(false);
    }
}

[Serializable]
public class MessageInfo : WriterMessage
{
    public string name;

    public bool closeWindow;

    public AudioClip letterSound;

    public Sprite image;

    public MessageBoxManager.DialogBoxPosition position;

    public string textEffectTypeName;

    public MessageInfo() : base()
    {
        name = string.Empty;
        textEffectTypeName = "None";

        closeWindow = false;

        letterSound = null;
        image = null;

        position = MessageBoxManager.DialogBoxPosition.Bottom;
    }
}