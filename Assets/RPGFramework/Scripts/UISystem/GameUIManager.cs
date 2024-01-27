using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : UIManagerBase
{
    public UIPageBase TestPage;

    public override void Initialize()
    {
        SetPage(TestPage);
    }
}
