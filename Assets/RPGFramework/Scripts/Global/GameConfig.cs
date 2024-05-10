using System;
using UnityEngine;

[Serializable]
public struct GameConfig
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