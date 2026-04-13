using UnityEngine;

namespace RPGF.RPG
{
    public abstract class RPGBase : ScriptableObject
    {
        public string Tag;
        public string Name;
        [TextArea(2, 6)]
        public string Description;

        public virtual string GetLocaleNameTag()
        {
            return $"{Tag}_Name";
        }

        public virtual string GetLocaleDesciptionTag()
        {
            return $"{Tag}_Desciption";
        }
    }
}
