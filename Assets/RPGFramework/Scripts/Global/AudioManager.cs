using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("Источники:")]
    [SerializeField] 
    private AudioSource BGMSource;
    [SerializeField]
    private AudioSource BGSSource;
    [SerializeField]
    private AudioSource MESource;
    [SerializeField]
    private AudioSource SESource;

    public AudioClip BGMClip => BGMSource.clip;
    public AudioClip BGSClip => BGSSource.clip;

    public bool BGMIsPlaying => BGMSource.isPlaying; 
    public bool BGSIsPlaying => BGSSource.isPlaying; 
    public bool MEIsPLaying => MESource.isPlaying;

    public bool BGMIsFade => fadeBGMCoroutine != null;
    public bool BGSIsFade => fadeBGSCoroutine != null;
    public bool MEIsFade => fadeMECoroutine != null;

    public float BGMVolume => BGMSource.volume;
    public float BGSVolume => BGMSource.volume;
    public float MEVolume => BGMSource.volume;
    public float SEVolume => BGMSource.volume;


    private Coroutine fadeBGMCoroutine;
    private Coroutine fadeBGSCoroutine;
    private Coroutine fadeMECoroutine;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) PauseBGM(0.2f);
        else if (Input.GetKeyDown(KeyCode.D)) ResumeBGM();
    }

    #region BGM

    public void PlayBGM(AudioClip clip, float volume = 1, float fadeTime = 0)
    {
        BGMSource.clip = clip;

        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGMCoroutine);

            fadeBGMCoroutine = StartCoroutine(VolumeFadeCoroutine(BGMSource, 0, volume, fadeTime, i => fadeBGMCoroutine = null));
        }
        else
            BGMSource.volume = volume;

        BGMSource.Play();
    }

    public void ResumeBGM(float volume = 1, float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGMCoroutine);

            BGMSource.UnPause();

            fadeBGMCoroutine = StartCoroutine(VolumeFadeCoroutine(BGMSource, 0, volume, fadeTime, i => fadeBGMCoroutine = null));
        }
        else
            BGMSource.volume = volume;

        BGMSource.UnPause();
    }

    public void PauseBGM(float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGMCoroutine);

            fadeBGMCoroutine = StartCoroutine(VolumeFadeCoroutine(BGMSource, BGMSource.volume, 0, fadeTime, i =>
            {
                i.Pause();

                fadeBGMCoroutine = null;
            }));
        }
        else
            BGMSource.Pause();
    }

    public void StopBGM(float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGMCoroutine);

            fadeBGMCoroutine = StartCoroutine(VolumeFadeCoroutine(BGMSource, BGMSource.volume, 0, fadeTime, i =>
            {
                i.Stop();

                fadeBGMCoroutine = null;
            }));
        }
        else
            BGMSource.Stop();
    }

    public void ChangeBGMVolume(float volume, float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGMCoroutine);

            fadeBGMCoroutine = StartCoroutine(VolumeFadeCoroutine(BGMSource, BGMSource.volume, volume, fadeTime, i =>
            {
                fadeBGMCoroutine = null;
            }));
        }
        else
            BGMSource.volume = volume;
    }

    #endregion

    #region BGS

    public void PlayBGS(AudioClip clip, float volume = 1, float fadeTime = 0)
    {
        BGSSource.clip = clip;

        if (fadeTime > 0)
        {
            if (fadeBGSCoroutine != null)
                StopCoroutine(fadeBGSCoroutine);

            fadeBGSCoroutine = StartCoroutine(VolumeFadeCoroutine(BGSSource, 0, volume, fadeTime, i => fadeBGSCoroutine = null));
        }
        else
            BGSSource.volume = volume;

        BGSSource.Play();
    }

    public void ResumeBGS(float volume = 1, float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGSCoroutine != null)
                StopCoroutine(fadeBGSCoroutine);

            fadeBGSCoroutine = StartCoroutine(VolumeFadeCoroutine(BGSSource, 0, volume, fadeTime, i => fadeBGSCoroutine = null));
        }

        BGSSource.UnPause();
    }

    public void PauseBGS(float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGSCoroutine != null)
                StopCoroutine(fadeBGSCoroutine);

            fadeBGSCoroutine = StartCoroutine(VolumeFadeCoroutine(BGSSource, BGSSource.volume, 0, fadeTime, i =>
            {
                i.Pause();

                fadeBGSCoroutine = null;
            }));
        }
        else
            BGSSource.Pause();
    }

    public void StopBGS(float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGSCoroutine != null)
                StopCoroutine(fadeBGSCoroutine);

            fadeBGSCoroutine = StartCoroutine(VolumeFadeCoroutine(BGSSource, BGSSource.volume, 0, fadeTime, i =>
            {
                i.Stop();

                fadeBGSCoroutine = null;
            }));
        }
        else
            BGSSource.Stop();
    }

    public void ChangeBGSVolume(float volume, float fadeTime = 0)
    {
        if (fadeTime > 0)
        {
            if (fadeBGMCoroutine != null)
                StopCoroutine(fadeBGSCoroutine);

            fadeBGSCoroutine = StartCoroutine(VolumeFadeCoroutine(BGSSource, BGSSource.volume, volume, fadeTime, i =>
            {
                fadeBGSCoroutine = null;
            }));
        }
        else
            BGSSource.volume = volume;
    }

    #endregion

    #region ME

    public void PlayME(AudioClip clip, float volume = 1, float fadeTime = 0)
    {
        MESource.clip = clip;

        if (fadeTime > 0)
        {
            if (fadeBGSCoroutine != null)
                StopCoroutine(fadeMECoroutine);

            fadeMECoroutine = StartCoroutine(VolumeFadeCoroutine(MESource, 0, volume, fadeTime, i => fadeMECoroutine = null));
        }
        else
            MESource.volume = volume;

        MESource.Play();
    }

    #endregion

    #region SE

    public void PlaySE(AudioClip clip, float volume = 1)
    {
        SESource.clip = clip;

        SESource.volume = volume;

        SESource.Play();
    }

    #endregion

    public static IEnumerator VolumeFadeCoroutine(AudioSource source, float from, float to, float time, Action<AudioSource> onEnd = null)
    {
        source.volume = (float)from;

        float dif = (float)to - (float)from;

        float speed = (float)dif / (float)time;

        float deltatime = 0;

        while (deltatime < time)
        {
            source.volume += (float)speed * (float)Time.fixedDeltaTime;

            deltatime += (float)Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        source.volume = to;

        onEnd?.Invoke(source);
    }
}
