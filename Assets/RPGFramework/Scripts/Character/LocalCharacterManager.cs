using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalCharacterManager : MonoBehaviour, IManagerInitialize
{
    public RPGCharacter[] Characters => GameManager.Instance.character.Characters;

    public List<DynamicExplorerObject> Models = new List<DynamicExplorerObject>();
    public List<Vector2> Targets = new List<Vector2>();

    [SerializeField]
    private float updateDistance;
    [SerializeField]
    private float updateSpeed = 1f;
    [SerializeField]
    private float distance = 0;
    [SerializeField]
    private float moveTime = 1f;

    public void Initialize()
    {
        GameManager.Instance.character.OnCharaterListChanged += CharacterManager_OnCharaterListChanged;

        ExplorerManager.PlayerMovement.OnMoving += Movement_OnMoving;
        ExplorerManager.PlayerMovement.OnStopMoving += Movement_OnStopMoving;
        ExplorerManager.PlayerMovement.OnStartMoving += PlayerMovement_OnStartMoving;
        ExplorerManager.PlayerMovement.OnRotate += PlayerMovement_OnRotate;
    }

    public void AddModel(DynamicExplorerObject model)
    {
        if (Models.Contains(model))
            return;

        model.transform.SetParent(transform);

        Models.Add(model);
        Targets.Add(ExplorerManager.GetPlayerPosition());
    }

    public void RemoveModel(DynamicExplorerObject model)
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

            DynamicExplorerObject model = n.GetComponent<DynamicExplorerObject>();

            Models.Add(model);
            Targets.Add(ExplorerManager.GetPlayerPosition());
        }
    }

    private void Movement_OnStopMoving()
    {
        if (Models.Count == 0)
            return;

        for (int i = 0; i < Models.Count; i++)
        {
            if (i == 0)
                Models[i].StopAnimation(ExplorerManager.PlayerMovement.ViewDirection);
            else
                Models[i].PauseMove();
        }
    }

    private void Movement_OnMoving()
    {
        if (Models.Count == 0)
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
        if (Models.Count == 0)
            return;

        Models[0].AnimateMove(ExplorerManager.PlayerMovement.ViewDirection);

        for (int i = 1; i < Models.Count; i++)
        {
            if (Models[i].MoveInPause)
                Models[i].UnpauseMove();
        }
    }

    private void PlayerMovement_OnRotate(CommonDirection obj)
    {
        if (Models.Count == 0)
            return;

        Models[0].AnimateMove(obj);
    }

    private void CharacterManager_OnCharaterListChanged()
    {
        //UpdateModels();
    }


    private void OnDestroy()
    {
        GameManager.Instance.character.OnCharaterListChanged -= CharacterManager_OnCharaterListChanged;

        ExplorerManager.PlayerMovement.OnMoving -= Movement_OnMoving;
        ExplorerManager.PlayerMovement.OnStopMoving -= Movement_OnStopMoving;
        ExplorerManager.PlayerMovement.OnStartMoving -= PlayerMovement_OnStartMoving;
        ExplorerManager.PlayerMovement.OnRotate -= PlayerMovement_OnRotate;
    }
}
