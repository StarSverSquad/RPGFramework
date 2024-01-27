using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIElementBase : MonoBehaviour
{
    public UnityEvent OnFocus;
    public UnityEvent OnLostFocus;
    public UnityEvent OnAction;

    public void Focus()
    {
        OnFocus.Invoke();
    }

    public void Unfocus()
    {
        OnLostFocus.Invoke();
    }
}