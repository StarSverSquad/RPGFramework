using TMPro;
using UnityEngine;

namespace RPGF.Core.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutoTextMeshLocalize : RPGFrameworkBehaviour
    {
        [SerializeField]
        private bool _warnOnNotFound = true;

        private TextMeshProUGUI textMeshProUGUI;

        private bool isTryLocalize = false;

        public bool HasLocale { get; private set; } = false;

        private void OnEnable()
        {
            if (Game == null || !gameObject.activeInHierarchy)
                return;

            textMeshProUGUI = GetComponent<TextMeshProUGUI>();

            if (!isTryLocalize)
            {
                string localeText = string.Empty;

                HasLocale = Game.Localization.TryGetLocale(textMeshProUGUI.text, out localeText);

                textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                textMeshProUGUI.text = localeText;

                if (!HasLocale && _warnOnNotFound)
                {
                    Debug.LogWarning($"Localization not found for: {textMeshProUGUI.text}");
                }

                isTryLocalize = true;
            }
        }
    }


}