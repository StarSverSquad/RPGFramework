using RPGF.Core.VisualEffects.Abstractions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPGF.Core.Battle
{
    public class BattleAttackEffect : VisualEffectBase
    {
        [SerializeField]
        private Image spriteRenderer;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private Animator animator;

        [Space]
        [SerializeField]
        private string animatorTriggerName = "START";
        [SerializeField]
        private string animatorIdleStateName = "IDLE";

        [Space]
        [Tooltip("Ёффект будет происходить по центру экрана")]
        [SerializeField]
        private bool locateInCenter = false;
        public bool LocaleInCenter => locateInCenter;

        protected override IEnumerator EffectCoroutine()
        {
            animator.SetTrigger(animatorTriggerName);

            audioSource.Play();

            yield return new WaitForFixedUpdate();
            yield return new WaitWhile(() => !animator.GetCurrentAnimatorStateInfo(0).IsName(animatorIdleStateName));
        }
    }
}