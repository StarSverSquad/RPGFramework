using System;

namespace RPGF.Domain.DI
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InjectAttribute : Attribute
    {
    }
}
