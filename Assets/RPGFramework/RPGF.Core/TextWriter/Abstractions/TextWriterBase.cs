using RPGF.Core.TextEffecter.Abstractions;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using RPGF.Domain.TP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextWriter.Abstractions
{
    public abstract class TextWriterBase : RPGFrameworkBehaviour
    {
        public TextParser parser { get; protected set; }

        public WriterMessage BaseMessage { get; private set; }

        public bool IsSkiped { get; private set; }
        public bool IsPause { get; private set; }

        public bool IsWriting => writeCoroutine != null;

        public float defaultTextSpeed = 15;

        private Coroutine writeCoroutine;

        [SerializeField]
        protected TextMeshProUGUI textMeshPro;
        public TextMeshProUGUI TextMeshPro => textMeshPro;

        public event Action OnStartWritingCallback;
        public event Action OnEndWritingCallback;
        public event Action OnSkipedCallback;
        public event Action OnSpaceCallback;
        public event Action<char> OnEveryLetterCallback;
        public event Action<TextWriterActionBase> OnActionCallback;

        private List<TextEffectBase> textEffects = new();
        private string writeText = string.Empty;

        public override void Initialize()
        {
            textMeshPro.text = string.Empty;

            var metas = new List<UseTextActionAttribute>();
            var actions = GetType().Assembly
                               .GetTypes()
                               .Where(i => !i.IsAbstract && i.BaseType is not null && i.BaseType == typeof(TextWriterActionBase)
                                           && i.GetCustomAttribute<UseTextActionAttribute>() is not null)
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

            parser = new TextParser(allowedActions);
        }

        public void AddTextEffect(TextEffectBase effect, int startLetterIndex, int endLetterIndex)
        {
            effect.Invoke(this, startLetterIndex, endLetterIndex);

            textEffects.Add(effect);
        }

        public void InvokeWrite(WriterMessage message)
        {
            if (!IsWriting)
            {
                BaseMessage = message;

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

        public void CancelSkip()
        {
            IsSkiped = false;
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

            var textData = parser.ParseText(BaseMessage.text);
            textMeshPro.text = textData.ClearedText;
            textMeshPro.maxVisibleCharacters = 0;

            float letterDelay = 1f / (BaseMessage.speed <= 0 ? defaultTextSpeed : BaseMessage.speed);

            yield return null;

            writeText = textMeshPro.GetParsedText();

            var tags = textData.Tags.ToList();

            for (int index = 0; index < writeText.Length; index++)
            {
                if (writeText[index] == ' ')
                    OnSpaceCallback?.Invoke();

                foreach (var tag in tags.Where(t => t.RealIndex == index).ToList())
                {
                    yield return ProceedTag(tag, index, tags);
                }

                if (IsPause)
                {
                    OnWait();
                    yield return new WaitUntil(() => ContinueCanExecute());
                    IsPause = false;
                    OnEndWait();
                }

                textMeshPro.maxVisibleCharacters++;

                textMeshPro.ForceMeshUpdate();
                foreach (var item in textEffects)
                {
                    item.TextTransformer.ResetMeshOnlyChanges(item.StartLetter, item.EndLetter);
                }

                OnEveryLetter(writeText[index]);
                OnEveryLetterCallback?.Invoke(writeText[index]);

                if (!IsSkiped)
                    yield return new WaitForSeconds(letterDelay);
            }

            if (BaseMessage.wait)
            {
                OnWait();
                yield return new WaitUntil(() => ContinueCanExecute());
                OnEndWait();
            }

            IsSkiped = false;

            foreach (var effect in textEffects)
                effect.Stop();

            textEffects.Clear();

            writeCoroutine = null;

            OnEndWriting();
            OnEndWritingCallback?.Invoke();
        }

        private IEnumerator ProceedTag(TextParserTag tag, int index, List<TextParserTag> tags)
        {
            bool isMajorTag = tag.Type == TextParserTag.TagType.Single || tag.Type == TextParserTag.TagType.ScopedOpen;

            if (isMajorTag && tag.Action is TextWriterActionBase action)
            {
                string contains = string.Empty;
                int endIndex = index;
                bool skip = false;

                if (tag.Type == TextParserTag.TagType.ScopedOpen)
                {
                    var futureTags = tags.Where(t => t.RealIndex > tag.RealIndex).OrderBy(t => t.RealIndex);

                    int openCounter = 0;
                    TextParserTag closeTag = null;
                    foreach (var futureTag in futureTags)
                    {
                        if (futureTag.Type == TextParserTag.TagType.ScopedOpen)
                        {
                            openCounter++;
                        }
                        else if (futureTag.Type == TextParserTag.TagType.ScopedClose)
                        {
                            openCounter--;

                            if (openCounter <= 0 && futureTag.Tag == tag.Tag)
                            {
                                closeTag = futureTag;
                                break;
                            }
                        }
                    }

                    if (closeTag is not null)
                    {
                        endIndex = closeTag.RealIndex - 1;
                        contains = writeText[tag.RealIndex..endIndex];
                    }
                    else
                    {
                        skip = true;
                    }
                }

                if (!skip)
                {
                    OnActionCallback?.Invoke(action);

                    action.ClearReturnText();

                    yield return action.Invoke(this, new TextActionParams()
                    {
                        Contains = contains,
                        StartIndex = index, 
                        EndIndex = endIndex,
                        Tag = tag.Tag,
                        TagParams = tag.Params
                    });

                    if (!string.IsNullOrWhiteSpace(action.ReturnText))
                    {
                        var returnedTextData = parser.ParseText(action.ReturnText);

                        foreach (var item in tags.Where(t => t.RealIndex > tag.RealIndex))
                        {
                            item.RealIndex += returnedTextData.ClearedTextWithoutShadows.Length;
                        }
                        foreach (var subtag in returnedTextData.Tags)
                        {
                            subtag.RealIndex += index;
                        }

                        tags.AddRange(returnedTextData.Tags);

                        textMeshPro.text = writeText.Insert(index, returnedTextData.ClearedText);

                        yield return null;

                        writeText = textMeshPro.GetParsedText();
                    }
                }
            }
        }
    }
}