using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIElementBase : MonoBehaviour
{
    [Header("События")]
    public UnityEvent OnFocusEvent;
    public UnityEvent OnLostFocusEvent;
    public UnityEvent OnActionEvent;
    [Header("Переходы")]
    public UIElementBase UpTransmition;
    public UIElementBase DownTransmition;
    public UIElementBase LeftTransmition;
    public UIElementBase RightTransmition;

    public bool IsFocused { get; private set; }

    public void Focus()
    {
        if (IsFocused)
            return;

        IsFocused = true;

        OnFocus();
        OnFocusEvent.Invoke();
    }

    public void Unfocus()
    {
        if (!IsFocused)
            return;

        IsFocused = false;

        OnLostFocus();
        OnLostFocusEvent.Invoke();
    }

    public virtual bool CanSubmit()
    {
        return true;
    }

    public virtual bool CanTransmition(UISectionBase.TransmitionDirection direction)
    {
        return true;
    }

    public virtual void OnTransmition(UISectionBase.TransmitionDirection direction) { }

    public virtual void OnSubmit() { }

    public virtual void OnFocus() { }

    public virtual void OnLostFocus() { }

    private void OnDrawGizmosSelected()
    {
        if (UpTransmition != null)
            Debug.DrawLine(transform.position, UpTransmition.transform.position, Color.yellow);

        if (DownTransmition != null)
            Debug.DrawLine(transform.position, DownTransmition.transform.position, Color.yellow);

        if (LeftTransmition != null)
            Debug.DrawLine(transform.position, LeftTransmition.transform.position, Color.yellow);

        if (RightTransmition != null)
            Debug.DrawLine(transform.position, RightTransmition.transform.position, Color.yellow);
    }
}