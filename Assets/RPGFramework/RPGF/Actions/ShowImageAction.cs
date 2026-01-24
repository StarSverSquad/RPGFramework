using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Показать изображения", contextMenuPath: "Система/Показать изображения")]
    [Serializable]
    public class ShowImageAction : ActionBase
    {
        [Inject]
        private readonly MediaManager _media;

        [ActionFieldOption("Изображение:")]
        public Sprite ImageSprite;
        [ActionFieldOption("Время появление:")]
        public float FadeTime;
        [ActionFieldOption("Ждать?")]
        public bool IsWait;

        public ShowImageAction() : base()
        {
            ImageSprite = null;
            FadeTime = 0;

            IsWait = true;
        }

        public override IEnumerator ActionCoroutine()
        {
            _media.ShowImage(ImageSprite, FadeTime);

            if (IsWait)
                yield return new WaitWhile(() => _media.IsFade);
        }
    }
}
