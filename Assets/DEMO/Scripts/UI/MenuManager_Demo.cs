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

    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GameManager.Instance.NewGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame(0);
    }
}
