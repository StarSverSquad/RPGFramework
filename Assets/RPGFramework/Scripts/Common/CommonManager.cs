using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonManager : ContentManagerBase, IManagerInitialize
{
    public static CommonManager Instance;

    public MessageBoxManager MessageBox;
    public ChoiceBoxManager ChoiceBox;
    public MediaManager Media;

    public void Initialize()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        Media.Initialize();
    }
}
