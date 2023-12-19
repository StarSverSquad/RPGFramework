using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ContentManagerBase
{
    public static GameManager Instance;

    [Header("Глобальные ссылки")]
    public GlobalGameData gameData;
    public InventoryManager inventory;
    public GameUIManager GameUI;
    public GlobalCharacterManager characterManager;
    public GlobalLocationManager locationManager;
    public AudioManager gameAudio;
    public LoadingScreenManager loadingScreen;
    public SceneLoadManager sceneLoader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeChild();
        }
        else
            Destroy(gameObject);
    }

    public override void InitializeChild()
    {
        characterManager.Initialize();
    }
}
