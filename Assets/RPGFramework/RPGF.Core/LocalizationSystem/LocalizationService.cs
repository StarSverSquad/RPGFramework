using RPGF.Core.SaveLoad;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Localization
{
    public class LocalizationService
    {
        private readonly GameConfigService _gameConfig;

        private LocalizationSheet[] sheets;

        public LocalizationService(GameConfigService gameConfig)
        {
            _gameConfig = gameConfig;

            sheets = Resources.LoadAll<LocalizationSheet>("Localizations/").OrderBy(i => i.Order).ToArray();
        }

        public string GetLocale(string tag)
        {
            LocalizationLanguage language = _gameConfig.Config.Language;

            foreach (var sheet in sheets)
            {
                if (sheet.locales.HaveKey(tag))
                    return sheet.locales[tag].Get(language);
            }

            return tag;
        }

        public bool TryGetLocale(string tag, out string result)
        {
            var locale = GetLocale(tag);

            result = locale;

            return locale != tag;
        }
    }

}

