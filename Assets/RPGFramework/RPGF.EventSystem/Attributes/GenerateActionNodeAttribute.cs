using System;

namespace RPGF.EventSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GenerateActionNodeAttribute : Attribute
    {
        private string label;
        private string description;
        private string contextMenuPath;

        public string Label => label;
        public string Description => description;
        public string ContextMenuPath => contextMenuPath;

        public GenerateActionNodeAttribute(string label, string description = "", string contextMenuPath = "")
        {
            this.label = label;
            this.description = description;
            this.contextMenuPath = string.IsNullOrWhiteSpace(contextMenuPath) ? $"/{label}" : contextMenuPath;
        }
    }
}
