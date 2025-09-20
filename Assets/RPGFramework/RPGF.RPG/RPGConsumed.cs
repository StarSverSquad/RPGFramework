using System.Collections.Generic;
using UnityEngine;

namespace RPGF.RPG
{

    [CreateAssetMenu(fileName = "ConsumedItem", menuName = "RPG/ConsumedItem")]
    public class RPGConsumed : RPGCollectable
    {
        [Space]
        public bool WriteMessage = true;
    }
}
