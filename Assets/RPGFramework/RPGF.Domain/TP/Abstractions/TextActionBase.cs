using RPGF.Domain.DI;
using System.Collections;
using System.Collections.Generic;

namespace RPGF.Domain.TP.Abstractions
{
    public enum TextActionType
    {
        Single, Scoped
    }

    public class TextActionParams
    {
        public Dictionary<string, string> TagParams;

        public string Tag;

        public int StartIndex;
        public int EndIndex;

        public string Contains;
    }

    public abstract class TextActionBase : InjectionTarget
    {
        public string ReturnText { get; protected set; } = string.Empty;

        public abstract IEnumerator Action(TextActionParams @params);
    }
}