using RPGF.Domain.DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.Core.TextWriter
{
    public enum TextActionType
    {
        Instance, Scoped
    }

    public class TextActionParams
    {
        public Dictionary<string, string> TagParams;

        public string Tag;

        public int StartLetter;
        public int EndLetter;

        public string Contains;
    }

    public abstract class TextActionBase : InjectionTarget
    {
        [Inject]
        protected readonly TextWriterBase _textWriter;

        public string ReturnText { get; protected set; } = null;

        public Coroutine Invoke(MonoBehaviour listner, TextActionParams @params)
        {
            return listner.StartCoroutine(Action(@params));
        }

        protected abstract IEnumerator Action(TextActionParams @params);
    }
}