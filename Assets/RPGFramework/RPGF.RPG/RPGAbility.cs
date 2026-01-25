using RPGF.Core.Battle.Abstractions;
using RPGF.EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "Ability", menuName = "RPG/Ability")]
    public class RPGAbility : RPGUsable
    {
        [Space]
        public int ManaCost;
        public int ConcentrationCost;
        [Space]
        [Tooltip("Если положительный значит лечит, " +
                "если отрцатльный значит наносит урон. " +
                "Учитывает все внешние факторы.")]
        public int Formula;
        [Space]
        public GlobalEvent StartEvent;
        public GlobalEvent EndEvent;
        [Space]
        public MinigameBase Minigame;
    }
}
