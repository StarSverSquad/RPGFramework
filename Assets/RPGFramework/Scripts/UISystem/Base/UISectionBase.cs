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
    [Header("События")]
    public UnityEvent OnFocus;
    public UnityEvent OnLostFocus;
    [Space]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    [Space]
    public UnityEvent OnChange;
    public UnityEvent OnCancel;
    public UnityEvent OnAccept;
    [Space]
    [Header("Настройки")]
    public UIElementBase DefaultElement;

    private UISectionBase parent;
    public UISectionBase Parent => parent;

    private UISectionBase child;
    public UISectionBase Child => child;

    private UIElementBase currentElement;
    public UIElementBase CurrentElement => currentElement;

    private Coroutine focusCoroutine = null;
    public bool IsHaveFocus => focusCoroutine != null;

    protected bool isInitialized = false;
    public bool IsInitialized => isInitialized;

    public virtual void Initialize()
    {
        if (isInitialized)
            return;

        currentElement = DefaultElement;

        currentElement.Focus();

        isInitialized = true;

        OnEnter.Invoke();
    }
    public virtual void InitializeChild(UISectionBase child)
    {
        this.child = child;

        child.parent = this;

        Unfocus();

        child.Initialize();

        child.Focus();
    }
    public virtual void Deinitialize()
    {
        if (!isInitialized)
            return;

        if (currentElement != null)
            currentElement.Unfocus();

        if (child != null)
        {
            child.Deinitialize();
            child = null;
        }

        if (IsHaveFocus)
            Unfocus();

        if (parent != null)
        {
            parent.child = null;
            parent.Focus();
            parent = null;
        }

        isInitialized = false;

        OnExit.Invoke();
    }

    public virtual void Focus()
    {
        if (!IsHaveFocus)
            focusCoroutine = StartCoroutine(SectionCoroutine());

        if (currentElement != null)
            currentElement.Focus();

        OnFocus.Invoke();
    }
    public virtual void Unfocus()
    {
        if (IsHaveFocus)
        {
            StopCoroutine(focusCoroutine);

            focusCoroutine = null;
        }          

        OnLostFocus.Invoke();
    }

    public virtual void ChangeElementFocus(UIElementBase element)
    {
        currentElement.Unfocus();

        currentElement = element;

        currentElement.Focus();
    }

    public virtual bool CancelCanExecute() => Input.GetKeyDown(GameManager.Instance.GameConfig.Cancel);
    public virtual bool AcceptCanExecute() => Input.GetKeyDown(GameManager.Instance.GameConfig.Accept);

    public virtual bool TransmitionUp() => Input.GetKeyDown(GameManager.Instance.GameConfig.MoveUp);
    public virtual bool TransmitionDown() => Input.GetKeyDown(GameManager.Instance.GameConfig.MoveDown);
    public virtual bool TransmitionLeft() => Input.GetKeyDown(GameManager.Instance.GameConfig.MoveLeft);
    public virtual bool TransmitionRight() => Input.GetKeyDown(GameManager.Instance.GameConfig.MoveRight);

    protected virtual IEnumerator SectionCoroutine()
    {
        while (true)
        {
            if (CancelCanExecute())
            {
                yield return null;

                OnCancel.Invoke();
            }

            if (currentElement != null)
            {
                if (AcceptCanExecute())
                {
                    yield return null;

                    OnAccept.Invoke();

                    currentElement.OnAction.Invoke();
                }

                if (TransmitionUp() && currentElement.UpTransmition != null)
                {
                    yield return null;

                    ChangeElementFocus(currentElement.UpTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionDown() && currentElement.DownTransmition != null)
                {
                    yield return null;

                    ChangeElementFocus(currentElement.DownTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionLeft() && currentElement.LeftTransmition != null)
                {
                    yield return null;

                    ChangeElementFocus(currentElement.LeftTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionRight() && currentElement.RightTransmition != null)
                {
                    yield return null;

                    ChangeElementFocus(currentElement.RightTransmition);

                    OnChange.Invoke();
                }
            }

            yield return null;
        }
    }
}
