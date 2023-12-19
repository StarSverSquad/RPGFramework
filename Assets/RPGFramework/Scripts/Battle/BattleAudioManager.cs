using System.Collections;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource seSource;

    public float MusicVolume = 1;

    [SerializeField]
    private bool isFade = false;
    public bool IsFade => isFade;

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        MusicVolume = volume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic(float fadeTime = 0)
    {
        isFade = true;

        if (fadeTime == 0)
            musicSource.Stop();
        else
        {
            StartCoroutine(AudioManager.VolumeFadeCoroutine(musicSource, MusicVolume, 0, fadeTime, i =>
            {
                isFade = false;
                musicSource.Stop();
            }));
        }
    }

    public void PlaySound(AudioClip clip)
    {
        seSource.clip = clip;
        seSource.Play();
    }
}