using System;

namespace RPGF.Core.TextEffecter.Abstractions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UseTextEffectAttribute : Attribute
    {
        public string Title { get; set; }
        public string CodeName { get; set; }
    }
}
