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
        string[] typenames = GetType().Assembly.GetTypes()
                                      .Where(i => i.BaseType != null && i.BaseType.Name == "TextActionBase")
                                      .Select(i => i.Name)
                                      .ToArray();

        foreach (string typename in typenames)
        {
            TextActionBase actionBase = GetType().Assembly.CreateInstance(typename) as TextActionBase;

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
        string text = message.text.Clone() as string;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '<')
            {
                bool haveBreak = false;
                int count = 0;
                for (int j = i + 1; j < text.Length; j++)
                {
                    if (text[j] == '>')
                    {
                        haveBreak = true;
                        break;
                    }  

                    count++;
                }

                if (!haveBreak)
                    continue;

                string inner = text.Substring(i + 1, count);

                foreach (TextActionBase item in allActions)
                {
                    if (item.MatchRegex(inner))
                    {
                        TextActionBase action = (TextActionBase)Activator.CreateInstance(item.GetType());

                        action.TextWriter = this;

                        text = text.Remove(i, count + 2);

                        action.ParseText(inner);

                        switch (item.Type)
                        {
                            case TextActionBase.ActionType.TextReplace:
                                {
                                    text = text.Insert(i, action.GetText(inner));

                                    OnTextReplaceCallback?.Invoke(action);
                                }
                                break;
                            case TextActionBase.ActionType.TextAction:
                                {
                                    //text = text.Insert(i, ActionPoint.ToString());
                                    actionsIndex.Enqueue(i);
                                    actions.Enqueue(action);
                                }
                                break;
                        }

                        break;
                    }
                }
            }
        }

        outcomeText = text;
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

        previewText = textMeshPro.text;

        if (message.clear)
        {
            textMeshPro.text = outcomeText;
            textMeshPro.maxVisibleCharacters = 0;
        }
        else
        {
            textMeshPro.text = previewText + outcomeText;
            textMeshPro.maxVisibleCharacters = previewText.Length;
        }

        isSkiped = false;

        float letterDelay = 1f / (message.speed <= 0 ? defaultTextSpeed : message.speed);

        for (int i = 0; i < outcomeText.Length; i++)
        {
            if (outcomeText[i] == '<')
            {
                string txt = string.Empty;

                for (int j = i; j < outcomeText.Length; j++)
                {
                    txt += outcomeText[j];

                    if (outcomeText[j] == '>')
                        break;
                }

                i += txt.Length - 1;

                //textMeshPro.text += txt;
                //textMeshPro.maxVisibleCharacters += txt.Length;

                continue;
            }

            if (outcomeText[i] == ' ')
            {
                //textMeshPro.text += outcomeText[i];

                textMeshPro.maxVisibleCharacters++;

                OnSpaceCallback?.Invoke();

                continue;
            }
                

            if (isSkiped)
            {
                //textMeshPro.text = previewText + outcomeText.Replace(ActionPoint.ToString(), string.Empty);

                textMeshPro.maxVisibleCharacters = textMeshPro.text.Length;

                break;
            }

            if (actionsIndex.Count > 0 && i == actionsIndex.First())
            {
                TextActionBase act = actions.Dequeue();

                yield return act.Invoke(this);

                OnAction(act);
                OnActionCallback?.Invoke(act);

                actionsIndex.Dequeue();
            }
            else
            {
                //textMeshPro.text += outcomeText[i];

                textMeshPro.maxVisibleCharacters++;

                OnEveryLetter(outcomeText[i]);
                OnEveryLetterCallback?.Invoke(outcomeText[i]);

                yield return new WaitForSeconds(letterDelay);
            }

            if (isPause)
            {
                OnWait();
                yield return new WaitUntil(() => ContinueCanExecute());
                isPause = false;
                OnEndWait();
            }
        }

        if (message.wait)
        {
            OnWait();
            yield return new WaitUntil(() => ContinueCanExecute());
            OnEndWait();
        }

        actions.Clear();

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