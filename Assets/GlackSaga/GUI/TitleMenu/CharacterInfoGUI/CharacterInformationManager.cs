using System;
using DG.Tweening;
using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.GUI;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu
{
    class CharacterInformationManager : GUIBlock, IDisposable
    {
        [Inject]
        private readonly CharacterService _characterService = null!;

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
            UpdateInformation();
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

        public void UpdateInformation()
        {
            for (int i = 0; i < _charactersGUI.Length; i++)
            {
                if (i < _characterService.Characters.Length)
                {
                    _charactersGUI[i].gameObject.SetActive(true);
                    _charactersGUI[i].Initialize(_characterService.Characters[i]);
                }
                else
                {
                    _charactersGUI[i].gameObject.SetActive(false);
                }
            }
        }

        public override void Dispose()
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