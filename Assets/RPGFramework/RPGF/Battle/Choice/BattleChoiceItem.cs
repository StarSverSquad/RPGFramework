using RPGF.Core.Choice;
using RPGF.RPG;
using UnityEngine;

namespace RPGF.Battle.Choice
{
    public class BattleChoiceItem : ChoiceItem
    {
        public object Value { get; set; }

        public string Description { get; set; }
        public string CounterText { get; set; }
        public Sprite Icon { get; set; }

        public BattleChoiceItemUI Element { get; set; }

        public bool IsLocked { get; set; } = false;
    }
}
