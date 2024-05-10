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
    public enum TransmitionDirection
    {
        Up, Down, Left, Right
    }

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

            if (currentElement != null)
                currentElement.Unfocus();

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

    public virtual bool CancelCanExecute() => Input.GetKeyDown(GameManager.Instance.BaseOptions.Cancel);
    public virtual bool AcceptCanExecute() => Input.GetKeyDown(GameManager.Instance.BaseOptions.Accept);

    public virtual bool TransmitionUp() => Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveUp);
    public virtual bool TransmitionDown() => Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveDown);
    public virtual bool TransmitionLeft() => Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveLeft);
    public virtual bool TransmitionRight() => Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveRight);

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
                if (AcceptCanExecute() && currentElement.CanSubmit())
                {
                    yield return null;

                    currentElement.OnSubmit();

                    OnAccept.Invoke();

                    currentElement.OnActionEvent.Invoke();
                }

                if (TransmitionUp() && currentElement.UpTransmition != null && currentElement.CanTransmition(TransmitionDirection.Up))
                {
                    yield return null;

                    currentElement.OnTransmition(TransmitionDirection.Up);

                    ChangeElementFocus(currentElement.UpTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionDown() && currentElement.DownTransmition != null && currentElement.CanTransmition(TransmitionDirection.Down))
                {
                    yield return null;

                    currentElement.OnTransmition(TransmitionDirection.Down);

                    ChangeElementFocus(currentElement.DownTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionLeft() && currentElement.LeftTransmition != null && currentElement.CanTransmition(TransmitionDirection.Left))
                {
                    yield return null;

                    currentElement.OnTransmition(TransmitionDirection.Left);

                    ChangeElementFocus(currentElement.LeftTransmition);

                    OnChange.Invoke();
                }
                else if (TransmitionRight() && currentElement.RightTransmition != null && currentElement.CanTransmition(TransmitionDirection.Right))
                {
                    yield return null;

                    currentElement.OnTransmition(TransmitionDirection.Right);

                    ChangeElementFocus(currentElement.RightTransmition);

                    OnChange.Invoke();
                }
            }

            yield return null;
        }
    }
}
