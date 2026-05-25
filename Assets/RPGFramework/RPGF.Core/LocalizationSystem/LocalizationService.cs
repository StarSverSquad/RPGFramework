using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Localization
{
    public class LocalizationService : ISupportDI
    {
        [Inject]
        private readonly GameConfigService _gameConfig = null!;

        private LocalizationSheet[] sheets;

        public LocalizationService()
        {
            sheets = Resources.LoadAll<LocalizationSheet>("Localizations/").OrderBy(i => i.Order).ToArray();
        }

        /// <summary>
        /// Get localization text by localization tag
        /// </summary>
        /// <param name="tag">localization tag</param>
        /// <param name="fallback">returned if tag is not found (also return tag if null)</param>
        /// <returns>localized text</returns>
        public string GetLocale(string tag, string fallback = null)
        {
            LocalizationLanguage language = _gameConfig.Config.Language;

            foreach (var sheet in sheets)
            {
                if (sheet.locales.HaveKey(tag))
                    return sheet.locales[tag].Get(language);
            }

            return fallback ?? tag;
        }

        /// <summary>
        /// Try get localization text by localization tag
        /// </summary>
        /// <param name="tag">localization tag</param>
        /// <param name="result">localized text</param>
        /// <param name="fallback">returned if tag is not found (also return tag if null)</param>
        /// <returns>Is have localization key</returns>
        public bool TryGetLocale(string tag, out string result, string fallback = null)
        {
            var locale = GetLocale(tag, fallback);

            result = locale;

            return locale != tag;
        }
    }

}

