using RPGF.RPG;
using System;
using System.Collections;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    public abstract class RPGEffectBase
    {
        public float Factor { get; set; } = 1f;

        public virtual IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            yield break;
        }

        public abstract string GetName();

        public override string ToString()
        {
            return GetName();
        }
    }
}