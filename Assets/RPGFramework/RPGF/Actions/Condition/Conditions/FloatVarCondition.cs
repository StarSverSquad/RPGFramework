using RPGF.Domain.DI;

namespace RPGF.Actions.Condition
{
    [UseCondition("По дробной переменной")]
    public class FloatVarCondition : ConditionBase
    {
        [Inject]
        private readonly GameData _gameData;

        public string Var;
        public float Value;

        public ConditionOperation Operation;

        public FloatVarCondition()
        {
            Var = string.Empty;
            Value = 0;
            Operation = ConditionOperation.Equals;
        }

        public override bool Invoke()
        {
            if (!_gameData.FloatValues.HaveKey(Var))
                return false;

            return Operation switch
            {
                ConditionOperation.Equals => _gameData.FloatValues[Var] == Value,
                ConditionOperation.NotEquals => _gameData.FloatValues[Var] != Value,
                ConditionOperation.More => _gameData.FloatValues[Var] > Value,
                ConditionOperation.Less => _gameData.FloatValues[Var] < Value,
                ConditionOperation.MoreOrEquals => _gameData.FloatValues[Var] >= Value,
                ConditionOperation.LessOrEquals => _gameData.FloatValues[Var] <= Value,
                _ => false,
            };
        }
    }
}