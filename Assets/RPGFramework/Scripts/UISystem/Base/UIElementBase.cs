using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIElementBase : MonoBehaviour
{
    [Header("События")]
    public UnityEvent OnFocus;
    public UnityEvent OnLostFocus;
    public UnityEvent OnAction;
    [Header("Переходы")]
    public UIElementBase UpTransmition;
    public UIElementBase DownTransmition;
    public UIElementBase LeftTransmition;
    public UIElementBase RightTransmition;

    public void Focus()
    {
        OnFocus.Invoke();
    }

    public void Unfocus()
    {
        OnLostFocus.Invoke();
    }

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