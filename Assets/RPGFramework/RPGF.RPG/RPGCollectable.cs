using System;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "DefaultItem", menuName = "RPG/DefaultItem")]
    public class RPGCollectable : RPGUsable
    {
        public Rareness Rare;
    }
}
