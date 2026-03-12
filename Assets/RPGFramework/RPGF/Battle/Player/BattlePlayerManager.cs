using RPGF.Core;
using RPGF.Core.Battle.PlayerMode;
using RPGF.Domain.Interfaces;
using System;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerManager : RPGFrameworkBehaviour, IActive, IDisposable
    {
        [SerializeField]
        private GameObject container;

        public BattlePlayerBorder Border;
        public BattlePlayerInteraction Interaction;
        public BattlePlayerModeManager ModeManager;

        public PlayerModeData PlayerModeData;

        public override void Initialize()
        {
            ModeManager.Initialize();
        }

        public void SetActive(bool active)
        {
            container.SetActive(active);

            PlayerModeData.CanMove = active;
            PlayerModeData.CanAccelerate = active;

            if (active)
            {
                PlayerModeData.MoveSpeed = PlayerModeData.DefaultMoveSpeed;
                //transform.position = Battle.BattleField.StartPosition;
            }
        }

        public void Dispose()
        {
            ModeManager.Dispose();
        }
    }
}