using System;

namespace RPGF.Core.RPGEffect.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UseRPGEffectAttribute : Attribute
    {
        public string Label { get; }

        public UseRPGEffectAttribute(string label)
        {
            Label = label;
        }
    }
}
