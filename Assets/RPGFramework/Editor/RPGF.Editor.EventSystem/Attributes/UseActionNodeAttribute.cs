using System;

namespace RPGF.Editor.EventSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseActionNodeAttribute : Attribute
    {
        public string Label { get; }
        public string Description { get; }
        public string ContextualMenuPath { get; }

        public bool Ignore { get; set; } = false;

        public UseActionNodeAttribute(string label, string description = "", string contextualMenuPath = "")
        {
            Label = label;
            Description = description;
            ContextualMenuPath = string.IsNullOrWhiteSpace(contextualMenuPath) ? $"{label}" : contextualMenuPath;
        }
    }
}