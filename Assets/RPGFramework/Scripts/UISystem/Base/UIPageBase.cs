using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIPageBase : MonoBehaviour, IManagerInitialize
{
    [Header("События")]
    public UnityEvent OnOpenPage;
    public UnityEvent OnClosePage;
    [Header("Настройки")]
    public UISectionBase StartSection;
    [Space]
    public GameObject Content;

    public void Initialize()
    {
        Content.SetActive(true);

        OnOpenPage.Invoke();

        StartSection.Initialize();
        
        StartSection.Focus();
    }

    public void Deinitialize()
    {
        StartSection.Deinitialize();

        OnClosePage.Invoke();

        Content.SetActive(false);
    }
}
