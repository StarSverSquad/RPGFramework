using RPGF.Core;
using RPGF.Domain.Interfaces;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerManager : RPGFrameworkBehaviour, IActive
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
                transform.position = Battle.BattleField.StartPosition;
            }
        }
    }
}