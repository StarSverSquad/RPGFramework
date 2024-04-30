using System;
using System.Collections;
using UnityEngine;

public class InvokeAnimationAction : GraphActionBase
{
    public Animator ObjectAnimator;

    public string Trigger;

    public InvokeAnimationAction() : base("StartAnimation")
    {
        ObjectAnimator = null;

        Trigger = string.Empty;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (ObjectAnimator == null)
        {
            Debug.LogError("Аниматор не указан");

            yield break;
        }

        ObjectAnimator.SetTrigger(Trigger);

        yield return new WaitForFixedUpdate();
    }

    public override string GetHeader()
    {
        return "Запуск анимации";
    }
}