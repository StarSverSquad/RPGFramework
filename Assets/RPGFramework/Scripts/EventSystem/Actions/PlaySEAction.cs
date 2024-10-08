﻿using System.Collections;
using UnityEngine;

public class PlaySEAction : GraphActionBase
{
    public AudioClip clip;

    public float volume;

    public PlaySEAction() : base("PlaySE")
    {
        clip = null;
        volume = 1.0f;
    }

    public override IEnumerator ActionCoroutine()
    {
        GameManager.Instance.GameAudio.PlaySE(clip, volume);

        yield break;
    }

    public override string GetHeader()
    {
        return "Запуск SE";
    }

    public override object Clone()
    {
        return new PlaySEAction()
        {
            clip = clip,
            volume = volume
        };
    }
}