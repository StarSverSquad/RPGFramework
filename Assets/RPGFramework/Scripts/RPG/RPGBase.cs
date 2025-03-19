using UnityEngine;

namespace RPGF.RPG
{
    public abstract class RPGBase : ScriptableObject
    {
        public string Tag;
        public string Name;
        [TextArea(2, 6)]
        public string Description;
    }
}
