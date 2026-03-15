using RPGF.Domain.Interfaces;
using System;
using UnityEngine;

namespace RPGF.Core.Battle.PlayerMode.Interfaces
{
    public interface IPlayerMode : IActive, IDisposable
    {
        public PlayerModeEnum PlayerMode { get; }

        public Color SoulColor { get; }

        public PlayerModeData Data { get; set; }
    }
}
