using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameUIBase CurrentMenu = null;

    public List<GameUIBase> PreviewsMenu = new List<GameUIBase>();

    [SerializeField]
    private int MaxPreviewMenu = 5;

    public event Action<GameUIBase> OnManuChanged;

    public void ActivateMenu(GameUIBase menu)
    {
        if (PreviewsMenu.Count >= MaxPreviewMenu)
            PreviewsMenu.RemoveAt(MaxPreviewMenu - 1);

        PreviewsMenu.Insert(0, CurrentMenu);

        CurrentMenu.Disactivate();

        CurrentMenu = menu;
        CurrentMenu.Activate();

        OnManuChanged?.Invoke(menu);
    }

    public void ActivePreview()
    {
        if (PreviewsMenu.Count == 0)
            return;

        GameUIBase ui = PreviewsMenu[0];

        PreviewsMenu.RemoveAt(0);

        CurrentMenu.Disactivate();

        CurrentMenu = ui;
        CurrentMenu.Activate();

        OnManuChanged?.Invoke(ui);
    }
}
