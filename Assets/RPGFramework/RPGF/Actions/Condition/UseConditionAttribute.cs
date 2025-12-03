using System;

namespace RPGF.Actions.Condition
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UseConditionAttribute : Attribute
    {
        private readonly string _label;

        public string Label => _label;

        public UseConditionAttribute(string label)
        {
            _label = label;
        }
    }
}
