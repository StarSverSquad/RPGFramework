using RPGF.Localization;
using System;

namespace RPGF.SaveLoad
{
    [Serializable]
    public class GameConfigData
    {
        public float BGMVolume;
        public float BGSVolume;
        public float SEVolume;
        public float MEVolume;

        public int ResolutionX;
        public int ResolutionY;

        public bool Fullscreen;

        public LocalizationLanguage Language;
    }
}