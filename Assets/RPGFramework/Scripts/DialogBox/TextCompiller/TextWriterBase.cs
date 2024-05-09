using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class TextWriterBase : MonoBehaviour
{
    public const char ActionPoint = '\u1130';

    public List<TextActionBase> allActions = new List<TextActionBase>();

    public Queue<TextActionBase> actions = new Queue<TextActionBase>();
    public Queue<string> actionsTexts = new Queue<string>();

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

    protected virtual void Awake()
    {
        string[] typenames = GetType().Assembly.GetTypes().Where(i => i.BaseType != null && i.BaseType.Name == "TextActionBase").
                                                                    Select(i => i.Name).ToArray();

        foreach (string typename in typenames)
        {
            TextActionBase actionBase = GetType().Assembly.CreateInstance(typename) as TextActionBase;
            actionBase.TextWriter = this;

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

                foreach (var act in allActions)
                {
                    if (act.MatchRegex(inner))
                    {
                        text = text.Remove(i, count + 2);

                        if (act.ActType == TextActionBase.ActionType.TextReplace)
                        {
                            act.CalculateText(inner);

                            text = text.Insert(i, act.ResultText);

                            
                            OnTextReplaceCallback?.Invoke(act);
                        }
                        else
                        {
                            text = text.Insert(i, ActionPoint.ToString());

                            actions.Enqueue(act);
                            actionsTexts.Enqueue(inner);
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

        if (message.clear)
            textMeshPro.text = string.Empty;

        previewText = textMeshPro.text;

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
                textMeshPro.text += txt;

                continue;
            }

            if (outcomeText[i] == ' ')
            {
                textMeshPro.text += outcomeText[i];

                OnSpaceCallback?.Invoke();

                continue;
            }
                

            if (isSkiped)
            {
                textMeshPro.text = previewText + outcomeText.Replace(ActionPoint.ToString(), string.Empty);

                break;
            }

            if (outcomeText[i] == ActionPoint)
            {
                TextActionBase act = actions.Dequeue();
                string text = actionsTexts.Dequeue();

                act.CalculateText(text);

                yield return act.Invoke(this);

                OnAction(act);
                OnActionCallback?.Invoke(act);
            }
            else
            {
                textMeshPro.text += outcomeText[i];

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
        actionsTexts.Clear();

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