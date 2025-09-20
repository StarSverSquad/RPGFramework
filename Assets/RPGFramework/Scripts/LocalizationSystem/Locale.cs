using System;
using System.Linq;

namespace RPGF.Localization
{
    public enum LocalizationLanguage
    {
        ENG, RUS
    }

    [Serializable]
    public class Locale
    {
        [LocalizationField(LocalizationLanguage.RUS)]
        public string Russian;
        [LocalizationField(LocalizationLanguage.ENG)]
        public string English;

        public string Get(LocalizationLanguage language)
        {
            var fieldsWithAttribute = GetType().GetFields()
                .Where(field => field.CustomAttributes
                    .Any(attr => attr.AttributeType == typeof(LocalizationField)));

            
            foreach (var field in fieldsWithAttribute)
            {
                var attr = field.GetCustomAttributes(false)
                    .First(attr => attr.GetType() == typeof(LocalizationField)) as LocalizationField;

                if (attr.Language == language)
                    return field.GetValue(this) as string;
            }

            throw new LocalizationException(language);
        }
    }
}
