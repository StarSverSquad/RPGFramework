using RPGF.Domain;
using UnityEngine;

namespace RPGF.Core.Localization
{
    [CreateAssetMenu(fileName = "Localization", menuName = "RPGFramework/Localization")]
    public class LocalizationSheet : ScriptableObject
    {
        [Tooltip("Словарь локалей")]
        public CustomDictionary<Locale> locales = new();
        [Space]
        [Header("Настройки")]
        [Tooltip("Порядок")]
        public int Order = 0;
    }
}

