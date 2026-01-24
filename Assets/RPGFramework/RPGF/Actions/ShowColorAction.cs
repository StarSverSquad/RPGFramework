using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Отобразить цвет", contextMenuPath: "Система/Отобразить цвет")]
    [Serializable]
    public class ShowColorAction : ActionBase
    {
        [Inject]
        private readonly MediaManager _media;

        [ActionFieldOption("Цвет:")]
        public Color Color;
        [ActionFieldOption("Время появления?")]
        public float FadeTime;
        [ActionFieldOption("Ждать?")]
        public bool IsWait;

        public ShowColorAction() : base()
        {
            Color = Color.white;
            FadeTime = 0;

            IsWait = true;
        }

        public override IEnumerator ActionCoroutine()
        {
            _media.ShowColor(Color, FadeTime);

            if (IsWait)
                yield return new WaitWhile(() => _media.IsFade);
        }
    }
}
