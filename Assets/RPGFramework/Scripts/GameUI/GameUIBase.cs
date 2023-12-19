using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameUIBase : MonoBehaviour
{
    protected GameObject container; 

    public bool IsFocused => GameManager.Instance.GameUI.CurrentMenu == this;

    private bool uiActive;
    public bool UIActive => uiActive;

    public UnityEvent onActivate;
    public UnityEvent onDisactivate;

    private void Start()
    {
        Disactivate();
    }

    public void Activate()
    {
        container.SetActive(true);

        uiActive = true;

        OnActivate();

        onActivate?.Invoke();
    }


    public void Disactivate()
    {
        container.SetActive(false);

        uiActive = false;

        OnDisactivate();

        onDisactivate?.Invoke();
    }

    public virtual void OnActivate() { }
    public virtual void OnDisactivate() { }
}
