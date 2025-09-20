using DG.Tweening;
using RPGF.Core;
using System;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    public class MoneyGUI : RPGFrameworkBehaviour, IDisposable
    {
        [SerializeField]
        private TextMeshProUGUI _moneyText;
        [SerializeField]
        private RectTransform _selfRect;

        [SerializeField]
        private float _animationDuration = 0.2f;

        public override void Initialize()
        {
            _moneyText.text = $"{Game.GameData.Money} {GetLocale("SYS_MONEY")}";

            _selfRect.DOAnchorPosX(-130, _animationDuration).SetEase(Ease.OutCubic).From(new Vector2(130, -54)).Play();
        }

        public void Dispose()
        {
            _selfRect.DOAnchorPosX(130, _animationDuration).SetEase(Ease.OutCubic).From(new Vector2(-130, -54)).Play();
        }
    }
}

