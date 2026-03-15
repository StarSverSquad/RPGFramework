using RPGF.Core;
using RPGF.Core.Battle.PlayerMode;
using System;
using System.Linq;
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

        public PlayerModeEnum CurrentMode { get; private set; }

        public event Action<PlayerModeEnum> OnPlayerModeChanged;

        public override void Initialize()
        {
            foreach (var playerMode in playerModes)
            {
                playerMode.PreInitialize(data);
                Local.DI.InjectInto(playerMode);
            }

            SetMode(PlayerModeEnum.Soul);
        }

        public void SetMode(PlayerModeEnum mode)
        {
            playerModes.FirstOrDefault(i => i.PlayerMode == CurrentMode)?.Dispose();
            foreach (var playerMode in playerModes)
            {
                playerMode.SetActive(playerMode.PlayerMode == mode);
                if (playerMode.PlayerMode == mode)
                {
                    soulModel.color = playerMode.SoulColor;
                    playerMode.Initialize();
                }
            }

            CurrentMode = mode;

            OnPlayerModeChanged?.Invoke(mode);
        }

        public T GetMode<T>(PlayerModeEnum mode)
            where T : PlayerModeBase
        {
            return playerModes.FirstOrDefault(i => i.PlayerMode == mode) as T;
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