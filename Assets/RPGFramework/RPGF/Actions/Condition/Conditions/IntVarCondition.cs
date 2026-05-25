using RPGF;
using RPGF.Domain.DI;

namespace RPGF.Actions.Condition
{
    [UseCondition("По целочисленной переменной")]
    public class IntVarCondition : ConditionBase
    {
        [Inject]
        private readonly GameData _gameData = null!;

        public string Var;
        public int Value;

        public ConditionOperation Operation;

        public IntVarCondition()
        {
            Var = string.Empty;
            Value = 0;
            Operation = ConditionOperation.Equals;
        }

        public override bool Invoke()
        {
            if (!_gameData.IntValues.HaveKey(Var))
                return false;

            return Operation switch
            {
                ConditionOperation.Equals => _gameData.IntValues[Var] == Value,
                ConditionOperation.NotEquals => _gameData.IntValues[Var] != Value,
                ConditionOperation.More => _gameData.IntValues[Var] > Value,
                ConditionOperation.Less => _gameData.IntValues[Var] < Value,
                ConditionOperation.MoreOrEquals => _gameData.IntValues[Var] >= Value,
                ConditionOperation.LessOrEquals => _gameData.IntValues[Var] <= Value,
                _ => false,
            };
        }
    }
}
