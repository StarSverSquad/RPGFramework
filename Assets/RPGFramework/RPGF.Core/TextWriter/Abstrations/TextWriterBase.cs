using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextWriter.Abstrations
{
    public abstract class TextWriterBase : RPGFrameworkBehaviour
    {
        public TextParser _parser { get; protected set; }

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
        public event Action<TextWriterActionBase> OnActionCallback;
        public event Action<TextWriterActionBase> OnTextReplaceCallback;

        public override void Initialize()
        {
            var metas = new List<UseTextActionAttribute>();
            var actions = GetType().Assembly
                               .GetTypes()
                               .Where(i => i.GetCustomAttribute<UseTextActionAttribute>() is not null)
                               .Select(i =>
                               {
                                   metas.Add(i.GetCustomAttribute<UseTextActionAttribute>());
                                   var action = (TextWriterActionBase)Activator.CreateInstance(i);
                                   action.TextWriter = this;
                                   Local.DI.InjectInto(action);
                                   return action;
                               })
                               .ToArray();

            var allowedActions = metas.ToDictionary((meta) => actions[metas.IndexOf(meta)] as TextActionBase);

            _parser = new TextParser(allowedActions);
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

        public virtual void OnTextReplace(TextWriterActionBase action) { }

        public virtual void OnAction(TextWriterActionBase action) { }

        public virtual void OnWait() { }

        public virtual void OnEndWait() { }
             
        public abstract bool ContinueCanExecute();
        public abstract bool SkipCanExecute();

        private void Compilate()
        {
            string text = BaseMessage.text.Clone() as string;

            

            OutputText = text;
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

                //if (actionsIndex.Count > 0)
                //{
                //    int activeCount = 0;
                //    foreach (var item in actionsIndex.Where(i => index == i))
                //    {
                //        TextActionBase act = actions.ToArray()[activeCount];

                //        yield return act.Invoke(this);

                //        OnAction(act);
                //        OnActionCallback?.Invoke(act);

                //        activeCount++;
                //    }

                //    for (int i = 0; i < activeCount; i++)
                //    {
                //        actions.Dequeue();
                //        actionsIndex.Dequeue();
                //    }
                //}

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

            //actions.Clear();
            //actionsIndex.Clear();

            writeCoroutine = null;

            OnEndWriting();
            OnEndWritingCallback?.Invoke();
        }
    }
}