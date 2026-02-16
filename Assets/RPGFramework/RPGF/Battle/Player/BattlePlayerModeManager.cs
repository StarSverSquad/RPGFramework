using RPGF.Core;
using RPGF.Core.Battle.PlayerMode;
using System;
using UnityEngine;

namespace RPGF.Battle.Player
{
    public class BattlePlayerModeManager : RPGFrameworkBehaviour, IDisposable
    {
        [SerializeField]
        private PlayerModeData data;
        [SerializeField]
        private PlayerModeBase[] playerModes;
        [SerializeField]
        private SpriteRenderer soulModel;

        public event Action<PlayerModeEnum> OnPlayerModeChanged;

        public override void Initialize()
        {
            foreach (var playerMode in playerModes)
            {
                playerMode.Initialize(data);
            }

            SetMode(PlayerModeEnum.Soul);
        }

        public void SetMode(PlayerModeEnum mode)
        {
            foreach (var playerMode in playerModes)
            {
                playerMode.SetActive(playerMode.PlayerMode == mode);
                if (playerMode.PlayerMode == mode)
                {
                    soulModel.color = playerMode.SoulColor;
                }
            }

            OnPlayerModeChanged?.Invoke(mode);
        }

        public void Dispose()
        {
            foreach (var playerMode in playerModes)
            {
                playerMode.Dispose();
            }
        }
    }
}