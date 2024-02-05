using System.Collections;
using UnityEngine;

public class PlayMEAction : GraphActionBase
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
        GameManager.Instance.GameAudio.PlayME(clip, volume);

        yield break;
    }

    public override string GetHeader()
    {
        return "Запуск ME";
    }
}