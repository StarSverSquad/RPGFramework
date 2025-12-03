using RPGF;
using RPGF.Domain.DI;
using System;

namespace RPGF.Actions.Condition
{
    [Serializable]
    public abstract class ConditionBase : InjectionTarget
    {
        public abstract bool Invoke();
    }
}