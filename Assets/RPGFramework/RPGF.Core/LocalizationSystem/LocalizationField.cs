using System;

namespace RPGF.Core.Localization
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class LocalizationField : Attribute
    {
        public LocalizationLanguage Language { get; }

        public LocalizationField(LocalizationLanguage language)
        {
            Language = language;
        }
    }
}
