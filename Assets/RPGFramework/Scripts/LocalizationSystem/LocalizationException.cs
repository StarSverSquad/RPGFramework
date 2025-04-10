using System;

namespace RPGF.Localization
{
    public class LocalizationException : ApplicationException
    {
        public LocalizationLanguage language;

        public LocalizationException(LocalizationLanguage language) : base($"Ошибка локализации [Язык: {language}]")
        {
            this.language = language;
        }
    }
}
