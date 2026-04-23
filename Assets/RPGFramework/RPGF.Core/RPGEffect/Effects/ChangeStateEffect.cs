using RPGF.Battle;
using RPGF.Core.RPGEffect.Attributes;
using RPGF.RPG;
using System;
using System.Collections;
using System.Linq;

namespace RPGF.Core.RPGEffect
{
    [Serializable]
    [UseRPGEffect("Изменить состояние")]
    public class ChangeStateEffect : RPGEffectBase
    {
        public bool IsAddState = true;

        public RPGEntityState State;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            if (BattleManager.IsBattle
                && target is RPGCharacter character
                && BattleManager.Instance.Data.TurnsData.First(i => i.Character == character).IsDead)
                yield break;

            if (IsAddState)
            {
                target.AddState(State);
            }
            else
            {
                target.RemoveState(State);
            }

            yield break;
        }
    }
}