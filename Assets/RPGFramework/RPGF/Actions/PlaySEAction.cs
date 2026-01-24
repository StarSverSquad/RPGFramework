using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Запуск SE", "Запуск звукового эффекта", "Звук/Запуск SE")]
    [Serializable]
    public class PlaySEAction : ActionBase
    {
        [Inject]
        private readonly AudioManager _audio;

        [ActionFieldOption("Аудио:")]
        public AudioClip clip;
        [ActionFieldOption("Громкость:")]
        public float volume;

        public PlaySEAction() : base()
        {
            clip = null;
            volume = 1.0f;
        }

        public override IEnumerator ActionCoroutine()
        {
            _audio.PlaySE(clip, volume);

            yield break;
        }

        public override ActionBase Clone()
        {
            return new PlaySEAction()
            {
                clip = clip,
                volume = volume
            };
        }
    }
}