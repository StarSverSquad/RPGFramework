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
            && !GameManager.Instance.SceneLoader.IsLoading
            && Input.GetKeyDown(GameManager.Instance.GameConfig.Additional))
            SetPage(MainPage);
    }
}
