using RPGF.Battle;
using RPGF.RPG;
using System.Collections;
using System.Linq;

namespace RPGF.Core.RPGEffect
{
    public class ChangeStateEffect : RPGEffectBase
    {
        public bool IsAddState = true;

        public RPGEntityState State;

        public override IEnumerator Invoke(RPGEntity user, RPGEntity target)
        {
            if (BattleManager.IsBattle
                && target is RPGCharacter character
                && BattleManager._Data.TurnsData.First(i => i.Character == character).IsDead)
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

        public override string GetName()
        {
            return "Изменить состояние";
        }
    }
}