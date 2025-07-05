using RPGF.Localization;
using System;
using System.Linq;
using UnityEngine;

namespace RPGF.SaveLoad
{
    [Serializable]
    public class GameConfigService
    {
        private readonly GameFilesService _gameFiles;
        private readonly AudioManager _audio;

        public GameConfigData Config { get; private set; }


        public event Action OnConfigUpdated;

        public GameConfigService(GameFilesService gameFiles, AudioManager audio)
        {
            _gameFiles = gameFiles;
            _audio = audio;
        }

        public void Apply()
        {
            _audio.SetBGMMixerVolume(Config.BGMVolume);
            _audio.SetBGSMixerVolume(Config.BGSVolume);
            _audio.SetSEMixerVolume(Config.SEVolume);
            _audio.SetMEMixerVolume(Config.MEVolume);

            Screen.SetResolution(Config.ResolutionX, Config.ResolutionY, Config.Fullscreen);
        }

        public void Load()
        {
            GameConfigData raw = _gameFiles.LoadConfig();

            if (raw is not null)
                Config = raw;
            else
            {
                Config = CreateNew();
                Save();
            }

            OnConfigUpdated?.Invoke();
        }

        public void LoadAndApply()
        {
            Load();
            Apply();
        }

        public void Save()
        {
            _gameFiles.SaveConfig(Config);

            OnConfigUpdated?.Invoke();
        }

        public GameConfigData CreateNew()
        {
            LocalizationLanguage localization;

            Resolution actual = Screen.resolutions.OrderByDescending(i => i.width).First();

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

            return new GameConfigData()
            {
                BGMVolume = 1,
                BGSVolume = 1,
                SEVolume = 1,
                MEVolume = 1,

                Language = localization,

                ResolutionX = actual.width,
                ResolutionY = actual.height,

                Fullscreen = true
            };
        }
    }
}