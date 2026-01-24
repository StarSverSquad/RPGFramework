using System;

namespace RPGF.EventSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ActionFieldOptionAttribute : Attribute
    {
        private readonly string _label;

        public string Label => _label;

        /// <summary>
        /// Только для полей типа String
        /// </summary>
        public bool MultiLine { get; set; } = false;

        /// <summary>
        /// Только для полей типа UnityEngine.Object
        /// </summary>
        public bool AllowSceneObjects { get; set; } = false;

        public ActionFieldOptionAttribute(string label)
        {
            _label = label;
        }
    }
}
