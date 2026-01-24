using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using UnityEngine;

namespace RPGF.Actions.Condition
{
    [UseCondition("ѕо быстрым сохранени€м")]
    public class FastSaveCondition : ConditionBase
    {
        [Inject]
        private readonly FastSaveService _fastSave;

        public string Key;
        public int Value;

        public ConditionOperation Operation;

        public FastSaveCondition()
        {
            Key = string.Empty;
            Value = 0;
            Operation = ConditionOperation.Equals;
        }

        public override bool Invoke()
        {
            if (!_fastSave.HaveKey(Key))
            {
                Debug.LogWarning($"{Key} переменна€ не найдена в FastSaves!");
                return false;
            }

            int savedValue = _fastSave[Key];

            return Operation switch
            {
                ConditionOperation.Equals => savedValue == Value,
                ConditionOperation.LessOrEquals => savedValue <= Value,
                ConditionOperation.MoreOrEquals => savedValue >= Value,
                ConditionOperation.Less => savedValue < Value,
                ConditionOperation.More => savedValue > Value,
                ConditionOperation.NotEquals => savedValue != Value,
                _ => false
            };
        }
    }
}
