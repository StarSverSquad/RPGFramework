using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager_Demo : UIManagerBase
{
    public UIPageBase MainPage;

    public override void Initialize()
    {
        InitializeChild();

        SetPage(MainPage);
    }

    // Update is called once per frame
    void Update()
    {
        Initialize();
    }
}
