using TMPro;
using UnityEngine;

namespace RPGF.Core.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutoTextMeshLocalize : RPGFrameworkBehaviour
    {
        [SerializeField]
        private string translationTag = string.Empty;
        [SerializeField]
        private bool warningOnNotFound = true;

        public bool HasLocale { get; private set; } = false;

        private bool isTryLocalize = false;

        private void OnEnable()
        {
            if (Global == null || !gameObject.activeInHierarchy)
                return;

            if (!isTryLocalize && TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                HasLocale = Global.Localization.TryGetLocale(translationTag, out var localeText);
                textMeshProUGUI.text = localeText;

                if (!HasLocale && warningOnNotFound)
                {
                    Debug.LogWarning($"Localization not found for: {textMeshProUGUI.text}");
                }

                isTryLocalize = true;
            }
        }
    }
}