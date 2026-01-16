using JetBrains.Annotations;
using System;

namespace RPGF.Core.TextWriter
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UseTextWriterActionAttribute : Attribute
    {
        public TextActionType Type { get; }
        public string TagPattern { get; }

        public UseTextWriterActionAttribute([RegexPattern] string tagPattern, TextActionType type)
        {
            TagPattern = tagPattern;
            Type = type;
        }
    }
}
