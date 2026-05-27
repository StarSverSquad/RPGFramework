using System;
using UnityEngine;

namespace RPGF.RPG
{
    [CreateAssetMenu(fileName = "DefaultItem", menuName = "RPG/DefaultItem")]
    public class RPGCollectable : RPGUsable
    {
        public Rareness Rare;

        public override string GetLocaleNameTag()
        {
            return "Item_" + base.GetLocaleNameTag();
        }

        public override string GetLocaleDesciptionTag()
        {
            return "Item_" + base.GetLocaleDesciptionTag();
        }
    }
}
