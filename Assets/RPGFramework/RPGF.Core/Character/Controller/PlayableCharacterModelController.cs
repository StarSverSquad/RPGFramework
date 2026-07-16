using DG.Tweening;
using RPGF.Core.Enums;
using UnityEngine;

namespace RPGF.Core.Character
{
    [RequireComponent(typeof(Animator))]
    public class PlayableCharacterModelController : CharacterModelControllerBase
    {
        private Animator _animator;
        private SpriteRenderer[] _spriteRenderers;
        private Tween _visibilityTween;

        #region CONSTS

        public const string ANIM_PARAM_X = "X";
        public const string ANIM_PARAM_Y = "Y";

        public const string ANIM_PARAM_IsMove = "IsMove";
        public const string ANIM_PARAM_IsRun = "IsRun";

        public const string ANIM_PARAM_RESET = "RESET";

        #endregion

        public override void Initialize()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            base.Initialize();
        }

        public Tween SetVisibility(bool visible, float time)
        {
            DisposeVisibilityTween();

            float targetAlpha = visible ? 1f : 0f;

            if (_spriteRenderers.Length == 0 || time <= 0f)
            {
                SetVisibilityInstant(visible);
                return null;
            }

            Sequence sequence = DOTween.Sequence();

            foreach (var spriteRenderer in _spriteRenderers)
            {
                sequence.Join(spriteRenderer.DOFade(targetAlpha, time));
            }

            _visibilityTween = sequence;
            return sequence;
        }

        public void SetVisibilityInstant(bool visible)
        {
            DisposeVisibilityTween();

            float targetAlpha = visible ? 1f : 0f;

            foreach (var spriteRenderer in _spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = targetAlpha;
                spriteRenderer.color = color;
            }
        }

        public void StopVisibilityTween()
        {
            DisposeVisibilityTween();
        }

        private void DisposeVisibilityTween()
        {
            _visibilityTween?.Kill();
            _visibilityTween = null;
        }

        #region ANIMATION API

        public void SetRunAnimation(bool isRun)
        {
            _animator.SetBool(ANIM_PARAM_IsRun, isRun);
        }
        public void SetMoveAnimation(bool isMove)
        {
            _animator.SetBool(ANIM_PARAM_IsMove, isMove);
        }
        public void SetRotationAnimation(ViewDirection direction)
        {
            Vector2 vector = DirectionHelper.GetVectorByViewDiretion(direction);

            _animator.SetFloat(ANIM_PARAM_X, vector.x);
            _animator.SetFloat(ANIM_PARAM_Y, vector.y);
        }

        public void InvokeAnimation(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }
        public void ResetAnimations()
        {
            _animator.SetTrigger(ANIM_PARAM_RESET);
        }

        #endregion

        #region OVERRIDES

        protected override void OnRotate(ViewDirection direction)
        {
            SetRotationAnimation(direction);
        }

        protected override void OnStartMove()
        {
            SetMoveAnimation(true);
        }
        protected override void OnEndMove()
        {
            SetMoveAnimation(false);
        }

        protected override void OnPauseMove()
        {
            SetMoveAnimation(false);
        }
        protected override void OnResumeMove()
        {
            SetMoveAnimation(true);
        }

        #endregion
    }
}