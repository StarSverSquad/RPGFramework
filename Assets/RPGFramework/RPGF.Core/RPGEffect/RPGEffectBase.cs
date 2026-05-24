using RPGF.RPG;
using System;
using System.Collections;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    public abstract class RPGEffectBase
    {
        /// <summary>
        /// It define power of effect
        /// </summary>
        public float Factor { get; set; } = 1f;

        public virtual IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            yield break;
        }
    }
}