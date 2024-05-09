using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameConfigManager
{
    public event Action OnConfigUpdated;

    public GameConfig Config { get; private set; }

    private SaveLoadManager saveLoad;

    public GameConfigManager(SaveLoadManager saveLoad)
    {
        this.saveLoad = saveLoad;
    }

    public void Apply()
    {
        GameManager.Instance.GameAudio.SetBGMMixerVolume(Config.BGMVolume);
        GameManager.Instance.GameAudio.SetBGSMixerVolume(Config.BGSVolume);
        GameManager.Instance.GameAudio.SetSEMixerVolume(Config.SEVolume);
        GameManager.Instance.GameAudio.SetMEMixerVolume(Config.MEVolume);

        Screen.SetResolution(Config.Resolution.width, Config.Resolution.height, Config.Fullscreen);
    }

    public void Load()
    {
        GameConfig? raw = saveLoad.LoadConfig();

        if (raw is not null)
        {
            Config = (GameConfig)raw;
        }
        else
        {
            Config = CreateNew();
            Save();
        }

        OnConfigUpdated?.Invoke();
    }

    public void Save()
    {
        saveLoad.SaveConfig(Config);

        OnConfigUpdated?.Invoke();
    }

    public GameConfig CreateNew()
    {
        LocalizationLanguage localization;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                localization = LocalizationLanguage.RUS;
                break;
            default:
            case SystemLanguage.English:
                localization = LocalizationLanguage.ENG;
                break;
        }

        return new GameConfig()
        {
            BGMVolume = 1,
            BGSVolume = 1,
            SEVolume = 1,
            MEVolume = 1,

            Language = localization,
            Resolution = Screen.resolutions.OrderBy(i => i.width).First(),
            Fullscreen = true
        };
    }
}