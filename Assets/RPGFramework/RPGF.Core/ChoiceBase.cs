using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ChoiceBase<T> : MonoBehaviour
{
    protected List<T> choices = new List<T>();
    public T[] Choices => choices.ToArray();

    [SerializeField]
    protected int index = 0;
    public int Index => index;

    public T Current => choices[index];

    public bool IsChoicing => coroutine != null;

    private Coroutine coroutine;

    public virtual void Choice(params T[] choices)
    {
        if (!IsChoicing)
        {
            index = 0;

            this.choices.Clear();
            this.choices.AddRange(choices);

            coroutine = StartCoroutine(ChoiceCoroutine());
        }
    }

    public virtual void OnStart() { }
    public virtual void OnEnd() { }
    public virtual void OnSellectChanged() { }


    /// <summary>
    /// for changing choice
    /// </summary>
    /// <returns>Current index changing number</returns>
    public abstract int SellectionChanging();

    public abstract bool ConfirmCanExecuted();

    public virtual void ForceConfirm()
    {
        if (IsChoicing)
        {
            StopCoroutine(coroutine);

            coroutine = null;
        }
    }

    private IEnumerator ChoiceCoroutine()
    {
        OnStart();

        while (true)
        {
            yield return null;

            int dif = SellectionChanging();

            if (index + dif > choices.Count - 1 || index + dif < 0)
                dif = 0;

            index += dif;

            if (dif != 0)
                OnSellectChanged();

            if (ConfirmCanExecuted())
                break;
        }

        OnEnd();

        coroutine = null;
    }
}
