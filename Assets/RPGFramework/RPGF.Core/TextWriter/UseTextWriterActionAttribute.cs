using System;

namespace RPGF.Core.TextWriter
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UseTextWriterActionAttribute : Attribute
    {
        public UseTextWriterActionAttribute()
        {
            
        }
    }
}
