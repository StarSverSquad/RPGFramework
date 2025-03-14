using RPGF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerManager : ContentManagerBase, IManagerInitialize
{
    public static ExplorerManager Instance;

    public ExplorerEventHandler EventHandler;
    public PlayerExplorerManager PlayerManager;
    public ExplorerItemConsumeManager ItemConsumer;

    public static PlayerExplorerMovement PlayerMovement => Instance.PlayerManager.movement;

    public void Initialize()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        
    }

    public static Vector2 GetPlayerPosition()
    {
        if (Instance == null)
            return Vector2.zero;

        return Instance.PlayerManager.transform.position;
    }
    public static Vector3 GetPlayerPosition3D()
    {
        if (Instance == null)
            return Vector3.zero;

        return Instance.PlayerManager.transform.position;
    }

    public static ViewDirection GetPlayerViewDirection()
    {
        return Instance.PlayerManager.movement.ViewDirection;
    }
}
