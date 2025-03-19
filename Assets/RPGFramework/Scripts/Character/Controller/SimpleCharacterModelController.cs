using UnityEngine;
using System.Collections;

namespace RPGF.Character
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleCharacterModelController : CharacterModelControllerBase
    {
        [Header("Ключевые кадры:")]
        [SerializeField]
        private Sprite _upIdle;
        [SerializeField]
        private Sprite _downIdle;
        [SerializeField]
        private Sprite _leftIdle;
        [SerializeField]
        private Sprite _rightIdle;

        private SpriteRenderer _spriteRenderer;

        public override void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            base.Initialize();
        }

        #region OVERRIDES

        protected override void OnRotate(ViewDirection direction)
        {
            switch (direction)
            {
                case ViewDirection.Up:
                    _spriteRenderer.sprite = _upIdle;
                    break;
                case ViewDirection.Down:
                    _spriteRenderer.sprite = _downIdle;
                    break;
                case ViewDirection.Left:
                    _spriteRenderer.sprite = _leftIdle;
                    break;
                case ViewDirection.Right:
                    _spriteRenderer.sprite = _rightIdle;
                    break;
            }
        }

        #endregion
    }
}