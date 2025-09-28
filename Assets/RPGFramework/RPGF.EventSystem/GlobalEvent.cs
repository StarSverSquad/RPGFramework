using RPGF.EventSystem.Graph;
using UnityEngine;

namespace RPGF.EventSystem
{
    [CreateAssetMenu(fileName = "GlobalEvent", menuName = "RPGFramework/GlobalEvent")]
    public class GlobalEvent : ScriptableObject
    {
        public GraphEvent Event;
    }
}
