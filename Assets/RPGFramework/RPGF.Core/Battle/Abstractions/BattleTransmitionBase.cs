using RPGF.Core;
using System.Collections;

namespace RPGF.Core.Battle.Abstractions
{
    public abstract class BattleTransmitionBase : RPGFrameworkBehaviour
    {
        /// <summary>
        /// Invokes when player enter to battle before first battle proccesing
        /// </summary>
        public abstract IEnumerator PartOne();

        /// <summary>
        /// Invokes after first battle proccesing
        /// </summary>
        public abstract IEnumerator PartTwo();
    }
}