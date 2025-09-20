using DG.Tweening;
using RPGF.Core;
using RPGF.GUI.Other;
using RPGF.RPG;
using TMPro;
using UnityEngine;

namespace RPGF.Battle
{
    class BattleUIEnemyInformation : RPGFrameworkBehaviour
    {
        [SerializeField]
        private float _showHideDuration = 0.5f;
        [SerializeField]
        private float _showPosition = 200f;

        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private TextMeshProUGUI _description;
        [SerializeField]
        private TextMeshProUGUI _healthCounter;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private LineBar _healthBar;

        private Tween showHideTween;

        public override void Initialize()
        {
            _root.gameObject.SetActive(false);
        }

        public void ShowInformation(RPGEnemy enemy)
        {
            _name.text = enemy.Name;
            _description.text = enemy.Description;
            _healthBar.SetValue((float)enemy.Heal / (float)enemy.MaxHeal);
            _healthCounter.text = $"{enemy.Heal} / {enemy.MaxHeal}";

            _root.gameObject.SetActive(true);

            DisposeTween();
            showHideTween = _root
                .DOAnchorPosY(_showPosition, _showHideDuration)
                .From(new Vector2(_root.anchoredPosition.x, 0))
                .SetEase(Ease.OutSine)
                .Play();
        }
        public void HideInformation()
        {
            DisposeTween();

            _root.gameObject.SetActive(false);
        }

        private void DisposeTween()
        {
            if (showHideTween != null)
            {
                showHideTween.Kill();
                showHideTween = null;
            }
        }

        private void OnDisable()
        {
            DisposeTween();
        }
    }
}