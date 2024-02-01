using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerManager : ContentManagerBase, IManagerInitialize
{
    public static ExplorerManager Instance;

    public ExplorerEventHandler eventHandler;
    public PlayerExplorerManager playerManager;
    public ExplorerItemConsumeManager ItemConsumer;

    public static PlayerExplorerMovement PlayerMovement => Instance.playerManager.movement;

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

        return Instance.playerManager.transform.position;
    }
    public static Vector3 GetPlayerPosition3D()
    {
        if (Instance == null)
            return Vector3.zero;

        return Instance.playerManager.transform.position;
    }

    public static CommonDirection GetPlayerViewDirection()
    {
        if (Instance == null)
            return CommonDirection.None;

        return Instance.playerManager.movement.ViewDirection;
    }
}
