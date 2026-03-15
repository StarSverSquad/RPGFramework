using RPGF.Core.Battle.PlayerMode.Interfaces;
using UnityEngine;

namespace RPGF.Core.Battle.PlayerMode
{
    public abstract class PlayerModeBase : RPGFrameworkBehaviour, IPlayerMode
    {
        public abstract PlayerModeEnum PlayerMode { get; }
        public abstract Color SoulColor { get; }

        public PlayerModeData Data { get; set; }

        public void PreInitialize(PlayerModeData data)
        {
            Data = data;
        }

        public void SetActive(bool active)
        {
            enabled = active;
        }

        public virtual void Dispose() { }
    }
}
