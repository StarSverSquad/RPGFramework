using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerManager : ContentManagerBase
{
    public static ExplorerManager instance;

    public ExplorerEventHandler eventHandler;
    public LocalLocationManager locationManager;
    public LocalCharacterManager characterManager;
    public PlayerExplorerManager playerManager;
    public MainCameraManager cameraManager;

    public static PlayerExplorerMovement PlayerMovement => instance.playerManager.movement;

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

    public static Vector2 GetPlayerPosition()
    {
        if (instance == null)
            return Vector2.zero;

        return instance.playerManager.transform.position;
    }
    public static Vector3 GetPlayerPosition3D()
    {
        if (instance == null)
            return Vector3.zero;

        return instance.playerManager.transform.position;
    }

    public static CommonDirection GetPlayerViewDirection()
    {
        if (instance == null)
            return CommonDirection.None;

        return instance.playerManager.movement.ViewDirection;
    }

    public static LocationObject GetCurrentLocation()
    {
        if (instance == null)
            return null;

        return instance.locationManager.Current;
    }
}
