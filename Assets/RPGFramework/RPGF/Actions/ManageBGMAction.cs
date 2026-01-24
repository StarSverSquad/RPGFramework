using RPGF.Domain.DI;
using RPGF.EventSystem;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class ManageBGMAction : ActionBase
    {
        public enum OperationType
        {
            Play, Pause, Stop, VolumeChange, Resume
        }

        [Inject]
        private readonly AudioManager _audio;

        public OperationType Operation;

        public bool IngoreIfThisClip;
        public bool WaitFade;
        public bool UseFade;

        public float FadeTime;

        public float Volume;

        public AudioClip clip;

        public ManageBGMAction() : base()
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
                    if (IngoreIfThisClip && _audio.BGMIsPlaying && _audio.BGMClip == clip)
                        yield break;

                    _audio.PlayBGM(clip, Volume, UseFade ? FadeTime : 0);
                    break;
                case OperationType.Pause:
                    _audio.PauseBGM(UseFade ? FadeTime : 0);
                    break;
                case OperationType.Stop:
                    _audio.StopBGM(UseFade ? FadeTime : 0);
                    break;
                case OperationType.VolumeChange:
                    _audio.ChangeBGMVolume(Volume, UseFade ? FadeTime : 0);
                    break;
                case OperationType.Resume:
                    _audio.ResumeBGM(Volume, UseFade ? FadeTime : 0);
                    break;
            }

            if (UseFade && WaitFade)
                yield return new WaitWhile(() => _audio.BGMIsFade);
        }
    }
}