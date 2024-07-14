using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class TextWriterBase : MonoBehaviour, IManagerInitialize
{
    public const char ActionPoint = '\u1130';

    public List<TextActionBase> allActions = new List<TextActionBase>();

    public Queue<TextActionBase> actions = new Queue<TextActionBase>();
    public Queue<int> actionsIndex = new Queue<int>();

    [SerializeField]
    protected WriterMessage message;

    [Multiline]
    [SerializeField]
    protected string outcomeText;

    [Tooltip("Букв в секунду")]
    public float defaultTextSpeed = 15;

    public bool IsWriting => writeCoroutine != null;

    private bool isSkiped = false;
    public bool IsSkiped => isSkiped;

    private bool isPause = false;
    public bool IsPause => isPause;

    private string previewText = string.Empty;

    private Coroutine writeCoroutine;

    [SerializeField]
    protected TextMeshProUGUI textMeshPro;

    public event Action OnStartWritingCallback;
    public event Action OnEndWritingCallback;
    public event Action OnSkipedCallback;
    public event Action OnSpaceCallback;
    public event Action<char> OnEveryLetterCallback;
    public event Action<TextActionBase> OnActionCallback;
    public event Action<TextActionBase> OnTextReplaceCallback;

    public virtual void Initialize()
    {
        string[] typenames = GetType()
            .Assembly.GetTypes()
            .Where(i => i.BaseType != null && i.BaseType.Name == "TextActionBase")
            .Select(i => i.Name)
            .ToArray();

        foreach (string typename in typenames)
        {
            TextActionBase actionBase =
                GetType().Assembly.CreateInstance(typename) as TextActionBase;

            allActions.Add(actionBase);
        }
    }

    public virtual void InvokeWrite(WriterMessage message)
    {
        if (!IsWriting)
        {
            this.message = message;

            Compilate();

            writeCoroutine = StartCoroutine(WriteCoroutine());
            StartCoroutine(SkipCoroutine());
        }
    }

    private void Compilate()
    {
        string rawText = message.text.Clone() as string;

        for (int index = 0, realIndex = 0; index < rawText.Length; index++, realIndex++)
        {
            while (index < rawText.Length - 1 && rawText[index] == '<')
            {
                int actionRegexLength = 0;

                for (int j = index + 1; j < rawText.Length; j++)
                {
                    if (rawText[j] == '>')
                        break;

                    actionRegexLength++;
                }

                int actionLength = actionRegexLength + 2;

                string actionRegex = rawText.Substring(index + 1, actionRegexLength);

                TextActionBase action = allActions.Find(act => act.MatchRegex(actionRegex));

                if (action != null)
                {
                    var actionInstance = (TextActionBase)Activator.CreateInstance(action.GetType());

                    actionInstance.TextWriter = this;

                    rawText = rawText.Remove(index, actionLength);

                    actionInstance.ParseText(actionRegex);

                    switch (action.Type)
                    {
                        case TextActionBase.ActionType.TextReplace:
                            {
                                rawText = rawText.Insert(
                                    index,
                                    actionInstance.GetText(actionRegex)
                                );

                                OnTextReplaceCallback?.Invoke(actionInstance);
                            }
                            break;
                        case TextActionBase.ActionType.TextAction:
                            {
                                actionsIndex.Enqueue(realIndex);
                                actions.Enqueue(actionInstance);
                            }
                            break;
                    }
                }
                else
                    index += actionLength;
            }
        }

        outcomeText = rawText;
    }

    public void PauseWrite() => isPause = true;

    public virtual void OnEveryLetter(char letter) { }

    public virtual void OnStartWriting() { }

    public virtual void OnEndWriting() { }

    public virtual void OnSpace() { }

    public virtual void OnTextReplace(TextActionBase act) { }

    public virtual void OnAction(TextActionBase act) { }

    public virtual void OnWait() { }

    public virtual void OnEndWait() { }

    public abstract bool ContinueCanExecute();
    public abstract bool SkipCanExecute();

    private IEnumerator SkipCoroutine()
    {
        while (IsWriting)
        {
            if (SkipCanExecute())
            {
                isSkiped = true;

                OnSkipedCallback?.Invoke();

                break;
            }

            yield return null;
        }
    }

    private IEnumerator WriteCoroutine()
    {
        OnStartWriting();
        OnStartWritingCallback?.Invoke();

        int startIndex = textMeshPro.GetParsedText().Length - 1;

        if (message.clear)
        {
            textMeshPro.text = outcomeText;
            textMeshPro.maxVisibleCharacters = 0;
            startIndex = 0;
        }
        else
        {
            textMeshPro.text += outcomeText;
            textMeshPro.maxVisibleCharacters = startIndex + 1;
        }

        float letterDelay = 1f / (message.speed <= 0 ? defaultTextSpeed : message.speed);

        yield return null;

        string parsedText = textMeshPro.GetParsedText();

        for (int index = startIndex; index < parsedText.Length; index++)
        {
            if (parsedText[index] == ' ')
                OnSpaceCallback?.Invoke();

            if (isSkiped)
            {
                textMeshPro.maxVisibleCharacters = parsedText.Length;
                break;
            }

            if (actionsIndex.Count > 0)
            {
                int activeCount = 0;
                foreach (var item in actionsIndex.Where(i => index == i))
                {
                    TextActionBase act = actions.ToArray()[activeCount];

                    yield return act.Invoke(this);

                    OnAction(act);
                    OnActionCallback?.Invoke(act);

                    activeCount++;
                }

                for (int i = 0; i < activeCount; i++)
                {
                    actions.Dequeue();
                    actionsIndex.Dequeue();
                }
            }

            if (isPause)
            {
                OnWait();
                yield return new WaitUntil(() => ContinueCanExecute());
                isPause = false;
                OnEndWait();
            }

            textMeshPro.maxVisibleCharacters++;

            OnEveryLetter(parsedText[index]);
            OnEveryLetterCallback?.Invoke(parsedText[index]);

            yield return new WaitForSeconds(letterDelay);
        }

        if (message.wait)
        {
            OnWait();
            yield return new WaitUntil(() => ContinueCanExecute());
            OnEndWait();
        }

        isSkiped = false;

        actions.Clear();
        actionsIndex.Clear();

        writeCoroutine = null;

        OnEndWriting();
        OnEndWritingCallback?.Invoke();
    }
}

[Serializable]
public class WriterMessage
{
    [Multiline]
    public string text;
    public float speed;

    public bool clear;
    public bool wait;

    public WriterMessage()
    {
        text = string.Empty;
        speed = 0;

        clear = true;
        wait = true;
    }
}
