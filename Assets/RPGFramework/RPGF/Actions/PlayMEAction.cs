using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Запуск ME", "Запуск музыкального эффекта", "Звук/Запуск ME")]
    [Serializable]
    public class PlayMEAction : ActionBase
    {
        [Inject]
        private readonly AudioManager _audio;

        [ActionFieldOption("Аудио:")]
        public AudioClip clip;
        [ActionFieldOption("Громкость:")]
        public float volume;

        public PlayMEAction() : base()
        {
            clip = null;
            volume = 1.0f;
        }

        public override IEnumerator ActionCoroutine()
        {
            _audio.PlayME(clip, volume);

            yield break;
        }
    }
}