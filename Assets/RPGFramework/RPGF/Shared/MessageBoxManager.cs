using RPGF.Core.TextWriter;
using RPGF.Core.TextWriter.Abstrations;
using RPGF.Domain.DI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGF.Shared
{
    public class MessageBoxManager : GenericTextWriterBase<MessageBoxInfo>
    {
        public enum DialogBoxPosition
        {
            Bottom, Center, Top
        }

        [Inject]
        private readonly BaseOptions _options;

        [Header("Dto")]
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

        public override void Initialize()
        {
            base.Initialize();

            arrow.SetActive(false);
            messageBox.gameObject.SetActive(false);
            nameBox.gameObject.SetActive(false);
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
         
        public override bool ContinueCanExecute()
        {
            return Input.GetKeyDown(_options.Accept);
        }

        public override bool SkipCanExecute()
        {
            return Input.GetKeyDown(_options.Cancel);
        }

        public override void OnEveryLetter(char letter)
        {
            if (Message.letterSound != null)
            {
                letterEffect.Play();
            }
        }

        public override void OnStartWriting()
        {
            letterEffect.clip = Message.letterSound;

            SetupDialog();
        }

        public override void OnEndWriting()
        {
            if (Message.closeWindow)
            {
                messageBox.gameObject.SetActive(false);
                nameBox.gameObject.SetActive(false);
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
    public class MessageBoxInfo : WriterMessage
    {
        public string name;

        public bool closeWindow;

        public AudioClip letterSound;

        public Sprite image;

        public MessageBoxManager.DialogBoxPosition position;

        public MessageBoxInfo() : base()
        {
            name = string.Empty;

            closeWindow = false;

            letterSound = null;
            image = null;

            position = MessageBoxManager.DialogBoxPosition.Bottom;
        }
    }
}