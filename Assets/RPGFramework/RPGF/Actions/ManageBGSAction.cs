using RPGF.Domain.DI;
using RPGF.EventSystem;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    public class ManageBGSAction : ActionBase
    {
        public enum OperationType
        {
            Play, Pause, Stop, VolumeChange, Resume
        }

        public OperationType Operation;

        [Inject]
        private readonly AudioManager _audio;

        public bool IngoreIfThisClip;
        public bool WaitFade;
        public bool UseFade;

        public float FadeTime;

        public float Volume;

        public AudioClip clip;

        public ManageBGSAction() : base()
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
                    if (IngoreIfThisClip && _audio.BGSIsPlaying
                        && _audio.BGSClip == clip)
                        yield break;

                    _audio.PlayBGS(clip, Volume, UseFade ? FadeTime : 0);
                    break;
                case OperationType.Pause:
                    _audio.PauseBGS(UseFade ? FadeTime : 0);
                    break;
                case OperationType.Stop:
                    _audio.StopBGS(UseFade ? FadeTime : 0);
                    break;
                case OperationType.VolumeChange:
                    _audio.ChangeBGSVolume(Volume, UseFade ? FadeTime : 0);
                    break;
                case OperationType.Resume:
                    _audio.ResumeBGS(Volume, UseFade ? FadeTime : 0);
                    break;
            }

            if (UseFade && WaitFade)
                yield return new WaitWhile(() => _audio.BGSIsFade);
        }
    }
}