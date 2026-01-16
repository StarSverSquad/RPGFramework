using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextWriter
{
    public abstract class TextWriterBase : RPGFrameworkBehaviour
    {
        public const char INSTANCE_ACTION_POINT_SYMBOL = '\u1130';
        public const char SCOPED_START_ACTION_POINT_SYMBOL = '\u1131';
        public const char SCOPED_END_ACTION_POINT_SYMBOL = '\u1132';

        public TextActionBase[] actions;

        public Stack<TextActionBase> actionsStack = new();

        public WriterMessage BaseMessage { get; private set; }

        public string OutputText { get; private set; } = string.Empty;

        public bool IsSkiped { get; private set; }
        public bool IsPause { get; private set; }

        public bool IsWriting => writeCoroutine != null;


        [Tooltip("Букв в секунду")]
        public float defaultTextSpeed = 15;

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

        public override void Initialize()
        {
            actions = GetType().Assembly
                               .GetTypes()
                               .Where(i => i.GetCustomAttribute(typeof(UseTextWriterActionAttribute)) is not null)
                               .Select(i =>
                               {
                                   var action = (TextActionBase)Activator.CreateInstance(i);
                                   Local.DI.InjectInto(action);
                                   return action;
                               })
                               .ToArray();
        }

        public void InvokeWrite(WriterMessage message)
        {
            if (!IsWriting)
            {
                this.BaseMessage = message;

                Compilate();

                writeCoroutine = StartCoroutine(WriteCoroutine());
                StartCoroutine(SkipCoroutine());
            }
        }

        public void PauseWrite() => IsPause = true;

        public void CancelWrite()
        {
            StopAllCoroutines();

            writeCoroutine = null;
        }

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

        private void Compilate()
        {
            string rawText = BaseMessage.text.Clone() as string;

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

                    TextActionBase action = actions.Find(act => act.MatchRegex(actionRegex));

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
                            case TextActionBase.ActionType.Instance:
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

            OutputText = rawText;
        }

        private IEnumerator SkipCoroutine()
        {
            while (IsWriting)
            {
                if (SkipCanExecute())
                {
                    IsSkiped = true;

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

            if (BaseMessage.clear)
            {
                textMeshPro.text = OutputText;
                textMeshPro.maxVisibleCharacters = 0;
                startIndex = 0;
            }
            else
            {
                textMeshPro.text += OutputText;
                textMeshPro.maxVisibleCharacters = startIndex + 1;
            }

            float letterDelay = 1f / (BaseMessage.speed <= 0 ? defaultTextSpeed : BaseMessage.speed);

            yield return null;

            string parsedText = textMeshPro.GetParsedText();

            for (int index = startIndex; index < parsedText.Length; index++)
            {
                if (parsedText[index] == ' ')
                    OnSpaceCallback?.Invoke();

                if (IsSkiped)
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

                if (IsPause)
                {
                    OnWait();
                    yield return new WaitUntil(() => ContinueCanExecute());
                    IsPause = false;
                    OnEndWait();
                }

                textMeshPro.maxVisibleCharacters++;

                OnEveryLetter(parsedText[index]);
                OnEveryLetterCallback?.Invoke(parsedText[index]);

                yield return new WaitForSeconds(letterDelay);
            }

            if (BaseMessage.wait)
            {
                OnWait();
                yield return new WaitUntil(() => ContinueCanExecute());
                OnEndWait();
            }

            IsSkiped = false;

            actions.Clear();
            actionsIndex.Clear();

            writeCoroutine = null;

            OnEndWriting();
            OnEndWritingCallback?.Invoke();
        }
    }
}