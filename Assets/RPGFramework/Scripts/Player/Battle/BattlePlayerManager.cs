using System;
using UnityEngine;

public class BattlePlayerManager : MonoBehaviour, IActive
{
    [SerializeField]
    private GameObject container;

    public BattlePlayerBorder border;
    public BattlePlayerInteraction interaction;
    public BattlePlayerMovement movement;

    public void SetActive(bool active)
    {
        container.SetActive(active);

        movement.CanMove = active;

        if (active)
        {
            movement.MoveSpeed = movement.DefaultMoveSpeed;
            transform.position = BattleManager.Instance.battleField.transform.position;
        }
    }
}