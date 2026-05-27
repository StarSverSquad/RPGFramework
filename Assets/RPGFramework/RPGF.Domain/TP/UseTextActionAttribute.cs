using System;
using RPGF.Domain.TP.Abstractions;

namespace RPGF.Domain.TP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UseTextActionAttribute : Attribute
    {
        public TextActionType Type { get; }
        public string TagPattern { get; }

        public UseTextActionAttribute(string tagPattern, TextActionType type)
        {
            TagPattern = tagPattern;
            Type = type;
        }
    }
}
