using System;

namespace RPGF.Editor.EventSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseActionNode : Attribute
    {
        public string ContextualMenuPath { get; }

        public UseActionNode(string contextualMenuPath = "")
        {
            ContextualMenuPath = contextualMenuPath;
        }
    }
}