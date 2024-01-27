using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UISectionBase : MonoBehaviour, IManagerInitialize
{
    public UnityEvent OnFocus;
    public UnityEvent OnLostFocus;
    [Space]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    [Space]
    public UnityEvent OnCancel;
    public UnityEvent OnAccept;
    [Space]
    public UISectionBase parent;
    public UISectionBase child;
    [Space]
    public UIElementBase DefaultElement;
    public UIElementBase CurrentElement;


    private Coroutine focusCoroutine = null;
    public bool IsHaveFocus => focusCoroutine != null;

    public virtual void Initialize()
    {
        CurrentElement = DefaultElement;

        OnEnter.Invoke();
    }
    public virtual void InitializeChild(UISectionBase child)
    {
        this.child = child;

        child.parent = this;

        child.Initialize();

        Unfocus();

        child.Focus();
    }
    public virtual void Deinitialize()
    {
        if (child != null)
        {
            child.Deinitialize();
            child = null;
        }

        if (IsHaveFocus)
            Unfocus();

        if (parent != null)
            parent.child = null;

        OnExit.Invoke();
    }

    public virtual void Focus()
    {

        Debug.Log("focus!");
        if (!IsHaveFocus)
            focusCoroutine = StartCoroutine(SectionCoroutine());

        OnFocus.Invoke();
    }
    public virtual void Unfocus()
    {
        if (IsHaveFocus)
            StopCoroutine(focusCoroutine);

        OnLostFocus.Invoke();
    }

    public virtual bool CancelCanExecute() => Input.GetKeyDown(GameManager.Instance.GameConfig.Cancel);
    public virtual bool AcceptCanExecute() => Input.GetKeyDown(GameManager.Instance.GameConfig.Accept);

    protected virtual IEnumerator SectionCoroutine()
    {
        while (true)
        {
            if (CancelCanExecute())
            {
                Debug.Log("ss");
                OnCancel.Invoke();
            }

            if (AcceptCanExecute())
            {
                OnAccept.Invoke();
                CurrentElement.OnAction.Invoke();
            }

            yield return null;
        }
    }
}
