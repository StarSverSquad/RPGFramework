using DG.Tweening;
using RPGF.Core;
using System;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    class CharacterInformationManager : RPGFrameworkBehaviour, IDisposable
    {
        [SerializeField]
        private CharacterInformationGUI[] _charactersGUI = new CharacterInformationGUI[4];
        [Space]
        [SerializeField]
        private RectTransform _root;
        [SerializeField]
        private float _animationTime;

        private Tween animTween;

        public override void Initialize()
        {
            for (int i = 0; i < _charactersGUI.Length; i++)
            {
                if (i < Game.Character.Characters.Length)
                {
                    _charactersGUI[i].gameObject.SetActive(true);
                    _charactersGUI[i].Initialize(Game.Character.Characters[i]);
                }
                else
                {
                    _charactersGUI[i].gameObject.SetActive(false);
                }
            }
        }

        public void Show()
        {
            DisposeTween();
            animTween = _root
                .DOAnchorPosY(_root.sizeDelta.y, _animationTime)
                .Play();
        }
        public void Hide()
        {
            DisposeTween();
            animTween = _root
                .DOAnchorPosY(0, _animationTime)
                .Play();
        }

        public void Dispose()
        {
            DisposeTween();
        }

        private void DisposeTween()
        {
            if (animTween != null)
            {
                animTween.Kill();
                animTween = null;
            }
        }
    }
}