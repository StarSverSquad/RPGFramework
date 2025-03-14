using RPGF.GUI;
using UnityEngine;

public class LocalManager : ContentManagerBase
{
    public static LocalManager Instance;

    [Header("Общие ссылки")]
    public MainCameraManager Camera;
    public LocalCharacterManager Character;
    public LocalLocationManager Location;
    public TittleMenuManager TittleMenu;
    public SunManager Sun;

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

    private void Update()
    {
        if (Input.GetKeyDown(GameManager.Instance.BaseOptions.Additional) 
            && !TittleMenu.IsOpened
            && !explorer.EventHandler.EventRuning)
            TittleMenu.Open();
    }

    public override void InitializeChild()
    {
        Camera.Initialize();
        TittleMenu.Initialize();

        explorer.Initialize();

        Location.Initialize();
        Character.Initialize();

        common.Initialize();
        battle.Initialize();
    }

    public static LocationController GetCurrentLocation()
    {
        if (Instance == null)
            return null;

        return Instance.Location.Current;
    }
}