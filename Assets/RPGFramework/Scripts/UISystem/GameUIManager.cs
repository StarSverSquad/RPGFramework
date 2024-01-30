using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : UIManagerBase
{
    public UIPageBase MainPage;

    private void Update()
    {
        if (!IsOpen 
            && !BattleManager.IsBattle 
            && !ExplorerManager.Instance.eventHandler.EventRuning
            && Input.GetKeyDown(GameManager.Instance.GameConfig.Additional))
            SetPage(MainPage);
    }
}
