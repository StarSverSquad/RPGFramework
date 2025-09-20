using UnityEngine;

namespace RPGF.Character
{
    [RequireComponent(typeof(Animator))]
    public class PlayableCharacterModelController : CharacterModelControllerBase
    {
        private Animator _animator;

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

            base.Initialize();
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
            Vector2 vector = DirectionConverter.GetVectorByViewDiretion(direction);

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