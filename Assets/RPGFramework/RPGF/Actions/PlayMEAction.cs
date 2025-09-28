using RPGF;
using RPGF.EventSystem;
using System.Collections;
using UnityEngine;

public class PlayMEAction : ActionBase
{
    public AudioClip clip;

    public float volume;

    public PlayMEAction() : base("PlayME")
    {
        clip = null;
        volume = 1.0f;
    }

    public override IEnumerator ActionCoroutine()
    {
        GlobalManager.Instance.GameAudio.PlayME(clip, volume);

        yield break;
    }

    public override string GetHeader()
    {
        return "Запуск ME";
    }
}