using RPGF.Domain.DI;

namespace RPGF.Actions.Condition
{
    [UseCondition("По строковой переменной")]
    public class StringVarCondition : ConditionBase
    {
        [Inject]
        private readonly GameData _gameData;

        public string Var;
        public string Value;

        public StringVarCondition()
        {
            Var = string.Empty;
            Value = string.Empty;
        }

        public override bool Invoke()
        {
            if (!_gameData.StringValues.HaveKey(Var))
                return false;

            return _gameData.StringValues[Var] == Value;
        }
    }
}
