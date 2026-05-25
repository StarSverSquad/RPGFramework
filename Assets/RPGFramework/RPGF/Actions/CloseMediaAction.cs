using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode(
        "Убрать медиа", 
        @"Убирает изображение из событий 'Отобразить цвет' и 'Отобразить фото'",
        "Система/Убрать медиа")]
    [Serializable]
    public class CloseMediaAction : ActionBase
    {
        [Inject]
        private readonly MediaManager _media = null!;

        [ActionFieldOption("Время затинения")]
        public float FadeTime;
        [ActionFieldOption("Ждать?")]
        public bool IsWait;

        public CloseMediaAction() : base()
        {
            FadeTime = 0;

            IsWait = true;
        }

        public override IEnumerator ActionCoroutine()
        {
            _media.HideImage(FadeTime);

            if (IsWait)
            {
                yield return new WaitWhile(() => _media.IsFade);
            }
        }
    }
}
