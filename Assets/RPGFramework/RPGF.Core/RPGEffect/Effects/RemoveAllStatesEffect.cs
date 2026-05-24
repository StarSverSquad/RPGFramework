using RPGF.Core.RPGEffect.Attributes;
using RPGF.RPG;
using System;
using System.Collections;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    [UseRPGEffect("Убрать все состояния")]
    public class RemoveAllStatesEffect : RPGEffectBase
    {
        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            target.RemoveAllStates();

            yield break;
        }
    }
}