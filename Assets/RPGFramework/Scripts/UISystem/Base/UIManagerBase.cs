using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIManagerBase : ContentManagerBase, IManagerInitialize
{
    [SerializeField]
    private UIPageBase currentPage;
    public UIPageBase CurrentPage => currentPage;

    public bool IsOn => currentPage != null;

    public virtual void Initialize()
    {
        
    }
    public override void InitializeChild()
    {
        
    }

    public void SetPage(UIPageBase page)
    {
        SetActive(true);

        if (currentPage != null)
        {
            currentPage.Deinitialize();
            currentPage.OnClosePage.Invoke();
        }

        currentPage = page;
        currentPage.Initialize();
    }

    public void Close()
    {
        Debug.Log("ss");

        currentPage.Deinitialize();
        
        currentPage = null;

        SetActive(false);
    }
}
