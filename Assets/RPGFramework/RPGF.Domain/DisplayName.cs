using System;
using UnityEngine;

namespace RPGF.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DisplayName : PropertyAttribute
    {
        public string Name;

        public DisplayName(string name)
        {
            Name = name;
        }
    }
}
