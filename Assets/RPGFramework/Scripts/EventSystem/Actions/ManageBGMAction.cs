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
                if (IngoreIfThisClip && GameManager.Instance.gameAudio.BGMIsPlaying
                    && GameManager.Instance.gameAudio.BGMClip == clip)
                    yield break;

                GameManager.Instance.gameAudio.PlayBGM(clip, Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Pause:
                GameManager.Instance.gameAudio.PauseBGM(UseFade ? FadeTime : 0);
                break;
            case OperationType.Stop:
                GameManager.Instance.gameAudio.StopBGM(UseFade ? FadeTime : 0);
                break;
            case OperationType.VolumeChange:
                GameManager.Instance.gameAudio.ChangeBGMVolume(Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Resume:
                GameManager.Instance.gameAudio.ResumeBGM(Volume, UseFade ? FadeTime : 0);
                break;
        }

        if (UseFade && WaitFade)
            yield return new WaitWhile(() => GameManager.Instance.gameAudio.BGMIsFade);
    }

    public override string GetHeader()
    {
        return "Управление BGM";
    }
}