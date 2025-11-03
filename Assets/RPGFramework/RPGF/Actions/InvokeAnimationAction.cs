using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Запуск анимации", "Запускает триггер выбранного аниматора", "Система/Запуск анимации")]
    public class InvokeAnimationAction : ActionBase
    {
        [ActionFieldOption("Аниматор:", AllowSceneObjects = true)]
        public Animator ObjectAnimator;
        [ActionFieldOption("Триггер:")]
        public string Trigger;

        public InvokeAnimationAction() : base()
        {
            ObjectAnimator = null;

            Trigger = string.Empty;
        }

        public override IEnumerator ActionCoroutine()
        {
            if (ObjectAnimator == null)
            {
                Debug.LogError("Animator is not assigned!");

                yield break;
            }

            ObjectAnimator.SetTrigger(Trigger);

            yield return new WaitForFixedUpdate();
        }
    }
}