using RPGF.Core.Localization;
using System;

namespace RPGF.Core.SaveLoad
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