using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalManager : ContentManagerBase
{
    public static LocalManager Instance;

    [Header("Общие ссылки")]
    public MainCameraManager Camera;
    public LocalCharacterManager Character;
    public LocalLocationManager Location;
    public GameUIManager GameUI;

    [Space]
    [Header("Ссылки для инициализации")]
    [SerializeField]
    private ExplorerManager explorer;
    [SerializeField]
    private CommonManager common;
    [SerializeField]
    private BattleManager battle;

    public void Start()
    {
        Instance = this;

        InitializeChild();
    }

    public override void InitializeChild()
    {
        GameUI.Initialize();

        explorer.Initialize();

        Location.Initialize();
        Character.Initialize();

        common.Initialize();
        battle.Initialize();
    }

    public static LocationObject GetCurrentLocation()
    {
        if (Instance == null)
            return null;

        return Instance.Location.Current;
    }
}