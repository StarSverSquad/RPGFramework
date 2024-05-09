using System.Linq;
using UnityEngine;

public class LocalizationManager
{
    private LocalizationSheet[] sheets;

    public LocalizationManager()
    {
        sheets = Resources.LoadAll<LocalizationSheet>("Localizations/");
    }

    public string GetLocale(string tag)
    {
        LocalizationLanguage language = GameManager.Instance.GameConfig.Config.Language;

        LocalizationSheet[] actualSheets = sheets.Where(i => i.Language == language || i.IsDefault)
                                                 .OrderByDescending(i => i.Order)
                                                 .ToArray();

        for (int i = 0; i < actualSheets.Length; i++)
        {
            if (actualSheets[i].locales.HaveKey(tag))
                return actualSheets[i].locales[tag];
        }

        return tag;
    }
}

public enum LocalizationLanguage
{
    ENG, RUS
}