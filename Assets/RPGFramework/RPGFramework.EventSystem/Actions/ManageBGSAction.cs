using System.Collections;
using UnityEngine;

public class ManageBGSAction : GraphActionBase
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

    public ManageBGSAction() : base("ManageBGS")
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
                if (IngoreIfThisClip && GameManager.Instance.GameAudio.BGSIsPlaying
                    && GameManager.Instance.GameAudio.BGSClip == clip)
                    yield break;

                GameManager.Instance.GameAudio.PlayBGS(clip, Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Pause:
                GameManager.Instance.GameAudio.PauseBGS(UseFade ? FadeTime : 0);
                break;
            case OperationType.Stop:
                GameManager.Instance.GameAudio.StopBGS(UseFade ? FadeTime : 0);
                break;
            case OperationType.VolumeChange:
                GameManager.Instance.GameAudio.ChangeBGSVolume(Volume, UseFade ? FadeTime : 0);
                break;
            case OperationType.Resume:
                GameManager.Instance.GameAudio.ResumeBGS(Volume, UseFade ? FadeTime : 0);
                break;
        }

        if (UseFade && WaitFade)
            yield return new WaitWhile(() => GameManager.Instance.GameAudio.BGSIsFade);
    }

    public override string GetHeader()
    {
        return "Управление BGS";
    }
}