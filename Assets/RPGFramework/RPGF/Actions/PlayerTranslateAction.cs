using System;
using System.Collections;
using RPGF.Core.Enums;
using RPGF.Domain;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Explorer;
using RPGF.Explorer.Player;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class PlayerTranslateAction : ActionBase
    {
        public enum TranslateType
        {
            Move, MoveRelative
        }

        [Inject]
        private readonly PlayerExplorerManager _playerManager = null!;

        public MoveDirection Direction;
        public TranslateType Type;

        public float Speed;

        public bool ReplaceInstance;
        public bool Wait;

        public Transform MovePoint;
        public Vector2 Offset;

        public PlayerTranslateAction() : base()
        {
            Direction = MoveDirection.Down;
            Type = TranslateType.MoveRelative;
            Speed = 1f;
            ReplaceInstance = false;
            Wait = false;
        }

        public override IEnumerator ActionCoroutine()
        {
            Vector3 playerPosition = ExplorerManager.GetPlayerPosition3D();

            switch (Type)
            {
                case TranslateType.Move:
                    if (!ReplaceInstance)
                        _playerManager.movement.TranslateBySpeed(MovePoint.transform.position - playerPosition, Speed);
                    else
                        _playerManager.TeleportToVector(MovePoint.transform.position);
                    break;
                case TranslateType.MoveRelative:
                    if (!ReplaceInstance)
                        _playerManager.movement.TranslateBySpeed(Offset, Speed);
                    else
                        _playerManager.TeleportToVector((Vector2)playerPosition + Offset);
                    break;
            }

            if (Wait)
                yield return new WaitWhile(() => _playerManager.movement.IsAutoMoving);

            yield break;
        }
    }
}