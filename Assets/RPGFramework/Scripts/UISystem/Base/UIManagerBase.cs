using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIManagerBase : ContentManagerBase, IManagerInitialize
{
    [SerializeField]
    private UIPageBase currentPage = null;
    public UIPageBase CurrentPage => currentPage;

    public bool IsOpen => currentPage != null;

    public virtual void Initialize()
    {
        InitializeChild();

        Close();
    }
    public override void InitializeChild()
    {
        foreach (UIPageBase page in GetComponentsInChildren<UIPageBase>())
        {
            page.Deinitialize();
        }
    }

    public void SetPage(UIPageBase page)
    {
        SetActive(true);

        if (currentPage != null)
            currentPage.Deinitialize();

        currentPage = page;
        currentPage.Initialize();
    }

    public void Close()
    {
        if (currentPage != null)
        {
            currentPage.Deinitialize();

            currentPage = null;
        }

        SetActive(false);
    }
}
