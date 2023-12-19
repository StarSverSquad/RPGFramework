using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonManager : ContentManagerBase
{
    public static CommonManager instance;

    public MessageBoxManager messageBox;
    public ChoiceBoxManager choiceBox;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            InitializeChild();
        }
    }

    public override void InitializeChild()
    {
        
    }
}
