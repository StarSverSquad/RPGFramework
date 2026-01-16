using RPGF.Domain.DI;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RPGF.Core.TextWriter
{
    public enum ActionType
    {
        Instance, Scoped
    }

    [Serializable]
    public abstract class TextActionBase : InjectionTarget
    {
        public TextWriterBase TextWriter;

        public ActionType Type { get; private set; }
        public Regex Regex { get; private set; }

        public TextActionBase(Regex regex, ActionType actType)
        {
            Regex = regex;
            Type = actType;
        }

        public Coroutine Invoke(MonoBehaviour listner)
        {
            return listner.StartCoroutine(Action());
        }

        public bool MatchRegex(string str) => Regex.IsMatch(str);

        /// <summary>
        /// Нужен для обработки текста внутри тега.
        /// Используеться для любой теговой обработки.
        /// </summary>
        public virtual void ParseText(string str) { }

        /// <summary>
        /// Нужен для обработки текста внутри тега и возращения нового.
        /// Используется для подмены текста.
        /// </summary>
        public virtual string GetText(string str) => "";

        protected virtual IEnumerator Action() { yield break; }
    }
}