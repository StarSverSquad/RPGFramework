using System.Collections;
using UnityEngine;

public class ManageBGMAction : GraphActionBase
{
    public enum OperationType
    {
        Play, Pause, Stop, VolumeChange, Resume
    }

    public OperationType Operation;

    public bool IngoreIfThisClip;
    public bool WaitFade;
    public bool UseFade;

    public float FadeTime;

    public float Volume;

    public AudioClip clip;

    public ManageBGMAction() : base("ManageBGM")
    {
        Operation = OperationType.Play;
        UseFade = false;
        FadeTime = 0.0f;
        Volume = 1.0f;
        clip = null;
        IngoreIfThisClip = true;
        WaitFade = false;
    }

    public override IEnumerator ActionCoroutine()
    {
        switch (Operation)
        {
            case OperationType.Play:
                if (IngoreIfThisClip && GameManager.Instance.GameAudio.BGMIsPlaying
                    && GameManager.Instance.GameAudio.BGMClip == clip)
                    yield break;

                GameManager.Instance.GameAudio.PlayBGM(clip, Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Pause:
                GameManager.Instance.GameAudio.PauseBGM(UseFade ? FadeTime : 0);
                break;
            case OperationType.Stop:
                GameManager.Instance.GameAudio.StopBGM(UseFade ? FadeTime : 0);
                break;
            case OperationType.VolumeChange:
                GameManager.Instance.GameAudio.ChangeBGMVolume(Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Resume:
                GameManager.Instance.GameAudio.ResumeBGM(Volume, UseFade ? FadeTime : 0);
                break;
        }

        if (UseFade && WaitFade)
            yield return new WaitWhile(() => GameManager.Instance.GameAudio.BGMIsFade);
    }

    public override string GetHeader()
    {
        return "Управление BGM";
    }
}