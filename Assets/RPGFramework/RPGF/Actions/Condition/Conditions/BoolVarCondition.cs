using RPGF.Domain.DI;
using System;

namespace RPGF.Actions.Condition
{
    [Serializable]
    [UseCondition("По переключателю")]
    public class BoolVarCondition : ConditionBase
    {
        [Inject]
        private readonly GameData _gameData = null!;

        public string Var;
        public bool Value;

        public BoolVarCondition()
        {
            Var = string.Empty;
            Value = false;
        }

        public override bool Invoke()
        {
            if (!_gameData.BoolValues.HaveKey(Var))
                return !Value;

            return _gameData.BoolValues[Var] == Value;
        }
    }
}
