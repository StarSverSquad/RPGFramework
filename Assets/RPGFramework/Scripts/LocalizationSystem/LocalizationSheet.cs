using UnityEngine;

[CreateAssetMenu(fileName = "Localization", menuName = "RPGFramework/Localization")]
public class LocalizationSheet : ScriptableObject
{
    public CustomDictionary<string> locales = new CustomDictionary<string>();

    [Header("Настройки")]
    public int Order = 0;

    public bool IsDefault = false;

    public LocalizationLanguage Language;
}