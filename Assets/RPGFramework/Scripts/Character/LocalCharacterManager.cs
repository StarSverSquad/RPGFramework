using System.Collections.Generic;
using UnityEngine;
using RPGF.Character;
using RPGF;
using RPGF.RPG;

public class LocalCharacterManager : RPGFrameworkBehaviour
{
    [SerializeField]
    private float _updateTargetsDistance = 1f;
    [SerializeField]
    private float _modelMoveTime = 1f;

    private List<PlayableCharacterModelController> models = new();
    private List<Vector2> targets = new();

    public RPGCharacter[] Characters => GameManager.Instance.Character.Characters;

    public List<PlayableCharacterModelController> Models => models;

    private PlayerExplorerMovement PlayerMovement => Explorer.PlayerManager.movement;
    private ExplorerEventHandler EventHandler => Explorer.EventHandler;

    public override void Initialize()
    {
        PlayerMovement.OnMoving += Movement_OnMoving;
        PlayerMovement.OnStopMoving += Movement_OnStopMoving;
        PlayerMovement.OnStartMoving += Movement_OnStartMoving;
        PlayerMovement.OnRotate += Movement_OnRotate;
        PlayerMovement.OnStartRun += Movement_OnStartRun;
        PlayerMovement.OnStopRun += Movement_OnStopRun;

        EventHandler.OnHandle += OnSomeEventStarted;
    }

    public void AddModel(PlayableCharacterModelController model)
    {
        if (models.Contains(model))
            return;

        model.transform.SetParent(transform);

        models.Add(model);
        targets.Add(ExplorerManager.GetPlayerPosition());
    }
    public void RemoveModel(PlayableCharacterModelController model)
    {
        if (!models.Contains(model))
            return;

        int index = models.IndexOf(model);

        models.Remove(model);
        targets.RemoveAt(index);
    }

    public void RebuildModels()
    {
        foreach (var item in models)
            Destroy(item.gameObject);

        models.Clear();
        targets.Clear();

        for (int i = 0; i < Characters.Length; i++)
        {
            var newObject = Instantiate(Characters[i].Model.gameObject, 
                ExplorerManager.GetPlayerPosition3D() + new Vector3(0, 0, 0.05f * i), 
                Quaternion.identity, transform);

            var model = newObject.GetComponent<PlayableCharacterModelController>();

            model.Initialize();

            models.Add(model);
            targets.Add(ExplorerManager.GetPlayerPosition());
        }
    }

    private void Movement_OnMoving()
    {
        if (models.Count == 0 || (EventHandler.HandledEvent && !PlayerMovement.IsAutoMoving))
            return;

        Vector2 playerPosition = ExplorerManager.GetPlayerPosition();

        float distance = Vector2.Distance(playerPosition, targets[0]);

        models[0].transform.position = playerPosition;
        
        if (distance > _updateTargetsDistance)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (i == 0)
                    targets[0] = playerPosition;
                else
                    targets[i] = targets[i - 1];
            }

            for (int i = 1; i < models.Count; i++)
            {
                models[i].MoveTo(targets[i - 1], _modelMoveTime);
            }
        }
    }
    private void Movement_OnStartMoving()
    {
        if (models.Count == 0 || (EventHandler.HandledEvent && !PlayerMovement.IsAutoMoving))
            return;

        Models[0].SetRotationAnimation(PlayerMovement.ViewDirection);
        Models[0].SetMoveAnimation(true);

        for (int i = 1; i < Models.Count; i++)
        {
            if (!Models[i].IsMove)
                continue;

            if (Models[i].MoveIsPaused)
                Models[i].ResumeMove();
        }
    }
    private void Movement_OnStopMoving()
    {
        if (models.Count == 0 || (EventHandler.HandledEvent && !PlayerMovement.IsAutoMoving))
            return;

        for (int i = 0; i < models.Count; i++)
        {
            if (i == 0)
                Models[i].SetMoveAnimation(false);
            else
                Models[i].PauseMove();
        }
    }

    private void Movement_OnRotate(ViewDirection direction)
    {
        if (models.Count == 0)
            return;

        Models[0].SetRotationAnimation(direction);
    }

    private void Movement_OnStopRun()
    {
        foreach (var item in models)
        {
            item.SetRunAnimation(false);
        }
    }
    private void Movement_OnStartRun()
    {
        foreach (var item in models)
        {
            item.SetRunAnimation(true);
        }
    }

    private void OnSomeEventStarted()
    {
        foreach (var model in models)
        {
            model.SetRotationAnimation(PlayerMovement.ViewDirection);
            model.SetMoveAnimation(false);
        }
    }

    private void OnDestroy()
    {
        PlayerMovement.OnMoving -= Movement_OnMoving;
        PlayerMovement.OnStopMoving -= Movement_OnStopMoving;
        PlayerMovement.OnStartMoving -= Movement_OnStartMoving;
        PlayerMovement.OnRotate -= Movement_OnRotate;
        PlayerMovement.OnStartRun -= Movement_OnStartRun;
        PlayerMovement.OnStopRun -= Movement_OnStopRun;

        EventHandler.OnHandle -= OnSomeEventStarted;
    }
}
