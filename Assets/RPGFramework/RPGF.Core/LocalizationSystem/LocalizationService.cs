using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Localization
{
    public class LocalizationService : ISupportDI
    {
        [Inject]
        private readonly GameConfigService _gameConfig;

        private LocalizationSheet[] sheets;

        public LocalizationService()
        {
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

