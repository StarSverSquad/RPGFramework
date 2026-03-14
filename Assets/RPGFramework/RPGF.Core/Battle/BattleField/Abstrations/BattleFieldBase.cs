using DG.Tweening;
using RPGF.Core.Helpers;
using RPGF.Domain.DI;
using System;
using UnityEngine;

namespace RPGF.Core.Battle.BattleField.Abstractions
{
    public abstract class BattleFieldBase : RPGFrameworkBehaviour, IDisposable
    {
        [Inject]
        private readonly BattleFieldManager _manager;

        protected Vector2 Center => _manager.Center;

        [Header("Ссылки:")]
        [SerializeField]
        private SpriteRenderer _field;
        [SerializeField]
        private AutoTiling _mask;
        [Header("Настройки:")]
        [SerializeField]
        private float animationTime = 0.1f;
        public float AnimationTime => animationTime;

        private Tween translateTween = null;
        private Tween rotateTween = null;
        private Tween resizeTween = null;

        protected Tween animationSizeTween = null;
        protected Tween animationColorTween = null;

        public override void Initialize()
        {
            
        }

        #region Move/Translate API

        public void MoveTo(Vector2 position)
        {
            translateTween?.Kill();

            transform.position = Center + position;
        }
        public void MoveTo(Vector2 position, float time, Ease easing = Ease.Linear)
        {
            translateTween?.Kill();

            translateTween = transform
                .DOMove(Center + position, time)
                .SetEase(easing)
                .SetLoops(-1)
                .Play();
        }

        public void Translate(Vector2 offset)
        {
            translateTween?.Kill();

            transform.position += (Vector3)offset;
        }
        public void Translate(Vector2 offset, float time, Ease easing = Ease.Linear)
        {
            translateTween?.Kill();

            translateTween = transform
                .DOMove(transform.position + (Vector3)offset, time)
                .SetEase(easing)
                .SetLoops(-1)
                .Play();
        }

        #endregion

        #region Rotate API

        public void Rotate(float angle)
        {
            rotateTween?.Kill();

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        public void Rotate(float angle, float time, Ease easing = Ease.Linear)
        {
            rotateTween?.Kill();

            transform.DORotate(new Vector3(0, 0, angle), time)
                .SetEase(easing)
                .SetLoops(-1)
                .Play();
                
        }
        public void Rotate360(float rotatesPerSecound, bool toRight = true)
        {
            rotateTween?.Kill();

            transform.DORotate(new Vector3(0, 0, 360 * (toRight ? -1 : 1)), 1f / rotatesPerSecound, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear)
                .Play();
        }

        #endregion

        #region Resize API

        public void Resize(Vector2 size)
        {
            resizeTween?.Kill();

            _field.size = size;
        }
        public void Resize(Vector2 size, float time, Ease Easing = Ease.Linear)
        {
            resizeTween?.Kill();

            resizeTween = DOTween
                .To(() => _field.size, (value) => _field.size = value, size, time)
                .SetEase(Easing)
                .SetLoops(-1);

            resizeTween.onUpdate = () =>
            {
                _mask.Tiling();
            };

            resizeTween.Play();
        }

        #endregion

        #region Show/Hide

        public virtual void Show()
        {
            animationSizeTween?.Kill();
            animationColorTween?.Kill();

            animationSizeTween = _field.transform
                .DOScale(1f, animationTime)
                .From(0.75f)
                .SetEase(Ease.Linear)
                .Play();

            animationColorTween = _field
                .DOFade(1, animationTime)
                .From(0)
                .SetEase(Ease.Linear)
                .Play();
        }

        public virtual void Hide()
        {
            animationSizeTween?.Kill();
            animationColorTween?.Kill();

            animationSizeTween = _field.transform
                .DOScale(0.75f, animationTime)
                .From(1f)
                .SetEase(Ease.Linear)
                .Play();

            animationColorTween = _field
                .DOFade(0, animationTime)
                .From(1)
                .SetEase(Ease.Linear)
                .Play();
        }

        #endregion

        public virtual void Dispose()
        {
            translateTween?.Kill();
            translateTween = null;

            rotateTween?.Kill();
            rotateTween = null;

            resizeTween?.Kill(); 
            resizeTween = null;

            animationColorTween?.Kill();
            animationSizeTween?.Kill();

            animationColorTween = null;
            animationSizeTween = null;
        }
    }
}
