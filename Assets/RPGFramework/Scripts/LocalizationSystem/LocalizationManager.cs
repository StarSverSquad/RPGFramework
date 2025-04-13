using System.Linq;
using UnityEngine;

namespace RPGF.Localization
{
    public class LocalizationManager
    {
        private readonly GameConfigManager _gameConfig;

        private LocalizationSheet[] sheets;

        public LocalizationManager(GameConfigManager gameConfig)
        {
            _gameConfig = gameConfig;

            sheets = Resources.LoadAll<LocalizationSheet>("Localizations/");
        }

        public string GetLocale(string tag)
        {
            LocalizationLanguage language = _gameConfig.Config.Language;

            LocalizationSheet[] actualSheets = sheets
                .OrderBy(i => i.Order)
                .ToArray();

            if (actualSheets.Length == 0)
                actualSheets = sheets
                    .Where(sheet => sheet.IsDefault)
                    .OrderBy(i => i.Order)
                    .ToArray();

            for (int i = 0; i < actualSheets.Length; i++)
            {
                if (actualSheets[i].locales.HaveKey(tag))
                    return actualSheets[i].locales[tag].Get(language);
            }

            return tag;
        }

        public bool TryGetLocale(string tag, out string result)
        {
            LocalizationLanguage language = _gameConfig.Config.Language;
            LocalizationSheet[] actualSheets = sheets
                .OrderBy(i => i.Order)
                .ToArray();

            if (actualSheets.Length == 0)
                actualSheets = sheets
                    .Where(sheet => sheet.IsDefault)
                    .OrderBy(i => i.Order)
                    .ToArray();

            for (int i = 0; i < actualSheets.Length; i++)
            {
                if (actualSheets[i].locales.HaveKey(tag))
                {
                    result = actualSheets[i].locales[tag].Get(language);
                    return true;
                }
            }

            result = tag;

            return false;
        }
    }

}

