using RPGF;
using RPGF.Domain.DI;

namespace RPGF.Actions.Condition
{
    [UseCondition("По деньгам")]
    public class MoneyCondition : ConditionBase
    {
        [Inject]
        private readonly GameData _gameData = null!;

        public int Value;

        public ConditionOperation Operation;

        public MoneyCondition()
        {
            Value = 0;
            Operation = ConditionOperation.Equals;
        }

        public override bool Invoke()
        {
            return Operation switch
            {
                ConditionOperation.Equals => _gameData.Money == Value,
                ConditionOperation.NotEquals => _gameData.Money != Value,
                ConditionOperation.More => _gameData.Money > Value,
                ConditionOperation.Less => _gameData.Money < Value,
                ConditionOperation.MoreOrEquals => _gameData.Money >= Value,
                ConditionOperation.LessOrEquals => _gameData.Money <= Value,
                _ => false,
            };
        }
    }
}
