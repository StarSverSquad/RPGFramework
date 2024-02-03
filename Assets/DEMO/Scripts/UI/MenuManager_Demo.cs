using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager_Demo : UIManagerBase
{
    public UIPageBase MainPage;

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        InitializeChild(); 
        
        SetPage(MainPage);
    }

    /*
     * 
     * Эти методы я создал для отлдки, можешь их изменить и тд.
     * Они привызаны к кнопкам на сцене.
     * 
     */

    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GameManager.Instance.saveLoad.NewGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.saveLoad.Load(1);
    }
}
