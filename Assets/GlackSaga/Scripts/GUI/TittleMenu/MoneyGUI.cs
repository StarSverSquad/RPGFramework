using DG.Tweening;
using RPGF.GUI;
using System;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    public class MoneyGUI : GUIElementBase, IDisposable
    {
        [SerializeField]
        private TextMeshProUGUI _moneyText;

        [SerializeField]
        private float _animationDuration = 0.2f;

        public override void Initialize()
        {
            base.Initialize();

            _moneyText.text = $"{Global.GameData.Money} {GetLocale("SYS_MONEY")}";

            RectTransform.DOAnchorPosX(-130, _animationDuration).SetEase(Ease.OutCubic).From(new Vector2(130, -54)).Play();
        }

        public override void Dispose()
        {
            RectTransform.DOAnchorPosX(130, _animationDuration).SetEase(Ease.OutCubic).From(new Vector2(-130, -54)).Play();
        }
    }
}

