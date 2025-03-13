using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalCharacterManager : RPGFrameworkBehaviour
{
    public RPGCharacter[] Characters => GameManager.Instance.Character.Characters;

    public List<RPGCharacterControllerLegacy> Models = new List<RPGCharacterControllerLegacy>();

    public List<Vector2> Targets = new List<Vector2>();

    [SerializeField]
    private float updateDistance;
    [SerializeField]
    private float updateSpeed = 1f;
    [SerializeField]
    private float moveTime = 1f;

    private float distance = 0;

    private PlayerExplorerMovement PlayerMovement => Explorer.PlayerManager.movement;
    private ExplorerEventHandler EventHandler => Explorer.EventHandler;

    public override void Initialize()
    {
        PlayerMovement.OnMoving += Movement_OnMoving;
        PlayerMovement.OnStopMoving += Movement_OnStopMoving;
        PlayerMovement.OnStartMoving += PlayerMovement_OnStartMoving;
        PlayerMovement.OnRotate += PlayerMovement_OnRotate;

        PlayerMovement.OnStartRun += PlayerMovement_OnStartRun;
        PlayerMovement.OnStopRun += PlayerMovement_OnStopRun;

        EventHandler.OnHandle += EventHandler_OnHandle;
    }

    private void PlayerMovement_OnStopRun()
    {
        foreach (var item in Models)
        {
            item.SpeedFactor = 1f;
        }
    }

    private void PlayerMovement_OnStartRun()
    {
        foreach (var item in Models)
        {
            item.SpeedFactor = 1.5f;
        }
    }

    public void AddModel(RPGCharacterControllerLegacy model)
    {
        if (Models.Contains(model))
            return;

        model.transform.SetParent(transform);

        Models.Add(model);
        Targets.Add(ExplorerManager.GetPlayerPosition());
    }

    public void RemoveModel(RPGCharacterControllerLegacy model)
    {
        if (!Models.Contains(model))
            return;

        int index = Models.IndexOf(model);

        Models.Remove(model);
        Targets.RemoveAt(index);
    }

    public void UpdateModels()
    {
        foreach (var item in Models)
        {
            Destroy(item.gameObject);
        }

        Models.Clear();
        Targets.Clear();

        distance = 0;

        for (int i = 0; i < Characters.Length; i++)
        {
            GameObject n = Instantiate(Characters[i].Model.gameObject, 
                ExplorerManager.GetPlayerPosition3D() + new Vector3(0, 0, 0.05f * i), 
                Quaternion.identity, transform);

            RPGCharacterControllerLegacy model = n.GetComponent<RPGCharacterControllerLegacy>();

            Models.Add(model);
            Targets.Add(ExplorerManager.GetPlayerPosition());
        }
    }

    private void Movement_OnStopMoving()
    {
        if (Models.Count == 0 || (EventHandler.HandledEvent && !PlayerMovement.IsAutoMoving))
            return;

        for (int i = 0; i < Models.Count; i++)
        {
            if (i == 0)
                Models[i].StopAnimation(PlayerMovement.ViewDirection);
            else
                Models[i].PauseMove();
        }
    }

    private void Movement_OnMoving()
    {
        if (Models.Count == 0 || (EventHandler.HandledEvent && !PlayerMovement.IsAutoMoving))
            return;

        float scalarvelocity = updateSpeed * Time.fixedDeltaTime;

        distance += scalarvelocity;

        Models[0].transform.position = ExplorerManager.GetPlayerPosition();
        
        if (distance > updateDistance)
        {
            for (int i = Targets.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                    Targets[0] = Models[0].transform.position;
                else
                    Targets[i] = Targets[i - 1];
            }

            for (int i = 1; i < Models.Count; i++)
            {
                Models[i].MoveToByTime(Targets[i - 1], moveTime);
            }

            distance = 0;
        }
    }

    private void PlayerMovement_OnStartMoving()
    {
        if (Models.Count == 0 || EventHandler.EventRuning)
            return;

        Models[0].AnimateMove(PlayerMovement.ViewDirection);

        for (int i = 1; i < Models.Count; i++)
        {
            if (Models[i].MoveInPause)
                Models[i].UnpauseMove();
        }
    }

    private void PlayerMovement_OnRotate(CommonDirection direction)
    {
        if (Models.Count == 0)
            return;

        Models[0].AnimateMove(direction);
    }

    private void EventHandler_OnHandle()
    {
        foreach (var model in Models)
        {
            model.StopAnimation(PlayerMovement.ViewDirection);
        }
    }

    private void OnDestroy()
    {
        PlayerMovement.OnMoving -= Movement_OnMoving;
        PlayerMovement.OnStopMoving -= Movement_OnStopMoving;
        PlayerMovement.OnStartMoving -= PlayerMovement_OnStartMoving;
        PlayerMovement.OnRotate -= PlayerMovement_OnRotate;

        EventHandler.OnHandle -= EventHandler_OnHandle;
    }
}
