using RPGF.RPG;
using System.Collections;

namespace RPGF.Core.RPGEffect
{
    public class RemoveAllStatesEffect : RPGEffectBase
    {
        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            target.RemoveAllStates();

            yield break;
        }

        public override string GetName()
        {
            return "Убрать все состояния";
        }
    }
}