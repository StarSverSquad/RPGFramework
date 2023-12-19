using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalCharacterManager : MonoBehaviour
{
    public RPGCharacter[] Characters => GameManager.Instance.characterManager.characters;

    public List<DynamicExplorerObject> models = new List<DynamicExplorerObject>();
    public List<Vector2> Targets = new List<Vector2>();

    [SerializeField]
    private float updateDistance;
    [SerializeField]
    private float updateSpeed = 1f;
    [SerializeField]
    private float distance = 0;
    [SerializeField]
    private float moveTime = 1f;

    private void Start()
    {
        GameManager.Instance.characterManager.OnCharaterListChanged += CharacterManager_OnCharaterListChanged;

        ExplorerManager.PlayerMovement.OnMoving += Movement_OnMoving;
        ExplorerManager.PlayerMovement.OnStopMoving += Movement_OnStopMoving;
        ExplorerManager.PlayerMovement.OnStartMoving += PlayerMovement_OnStartMoving;
        ExplorerManager.PlayerMovement.OnRotate += PlayerMovement_OnRotate;
    }

    public void UpdateModels()
    {
        foreach (var item in models)
        {
            Destroy(item.gameObject);
        }

        models.Clear();
        Targets.Clear();

        distance = 0;

        for (int i = 0; i < Characters.Length; i++)
        {
            GameObject n = Instantiate(Characters[i].Model.gameObject, 
                ExplorerManager.GetPlayerPosition3D() + new Vector3(0, 0, 0.05f * i), 
                Quaternion.identity);

            DynamicExplorerObject model = n.GetComponent<DynamicExplorerObject>();

            models.Add(model);
            Targets.Add(ExplorerManager.GetPlayerPosition());
        }
    }

    private void Movement_OnStopMoving()
    {
        if (models.Count == 0)
            return;

        for (int i = 0; i < models.Count; i++)
        {
            if (i == 0)
                models[i].StopAnimation(ExplorerManager.PlayerMovement.ViewDirection);
            else
                models[i].PauseMove();
        }
    }

    private void Movement_OnMoving()
    {
        if (models.Count == 0)
            return;

        float scalarvelocity = updateSpeed * Time.fixedDeltaTime;

        distance += scalarvelocity;

        models[0].transform.position = ExplorerManager.GetPlayerPosition();
        
        if (distance > updateDistance)
        {
            for (int i = Targets.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                    Targets[0] = models[0].transform.position;
                else
                    Targets[i] = Targets[i - 1];
            }

            for (int i = 1; i < models.Count; i++)
            {
                models[i].MoveToByTime(Targets[i - 1], moveTime);
            }

            distance = 0;
        }
    }

    private void PlayerMovement_OnStartMoving()
    {
        if (models.Count == 0)
            return;

        models[0].AnimateMove(ExplorerManager.PlayerMovement.ViewDirection);

        for (int i = 1; i < models.Count; i++)
        {
            if (models[i].MoveInPause)
                models[i].UnpauseMove();
        }
    }

    private void PlayerMovement_OnRotate(CommonDirection obj)
    {
        if (models.Count == 0)
            return;

        models[0].AnimateMove(obj);
    }

    private void CharacterManager_OnCharaterListChanged()
    {
        UpdateModels();
    }

    private void OnDrawGizmos()
    {
        foreach (var item in Targets)
        {
            Debug.DrawLine(item, item + new Vector2(0.1f, 0.1f), Color.green);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.characterManager.OnCharaterListChanged -= CharacterManager_OnCharaterListChanged;

        ExplorerManager.PlayerMovement.OnMoving -= Movement_OnMoving;
        ExplorerManager.PlayerMovement.OnStopMoving -= Movement_OnStopMoving;
        ExplorerManager.PlayerMovement.OnStartMoving -= PlayerMovement_OnStartMoving;
        ExplorerManager.PlayerMovement.OnRotate -= PlayerMovement_OnRotate;
    }
}
