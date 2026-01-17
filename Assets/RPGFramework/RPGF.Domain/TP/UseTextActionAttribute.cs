using JetBrains.Annotations;
using RPGF.Domain.TP.Abstractions;
using System;

namespace RPGF.Domain.TP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UseTextActionAttribute : Attribute
    {
        public TextActionType Type { get; }
        public string TagPattern { get; }

        public UseTextActionAttribute([RegexPattern] string tagPattern, TextActionType type)
        {
            TagPattern = tagPattern;
            Type = type;
        }
    }
}
