using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : ContentManagerBase
{
    public static GameManager Instance;

    [Header("Глобальные ссылки")]
    public GlobalGameData gameData;
    public InventoryManager inventory;
    public GlobalCharacterManager characterManager;
    public GlobalLocationManager locationManager;
    public AudioManager gameAudio;
    public LoadingScreenManager loadingScreen;
    public SceneLoadManager sceneLoader;

    private CommonGameConfig commonConfig;
    public CommonGameConfig CommonConfig => commonConfig;

    private void Awake()
    {
        if (Instance == null)
        {
            commonConfig = Resources.Load<CommonGameConfig>("Config");

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeChild();
        }
        else
            Destroy(gameObject);
    }

    public override void InitializeChild()
    {
        gameData.Initialize();
        characterManager.Initialize();
    }
}
