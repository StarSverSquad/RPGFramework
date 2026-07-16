using System;
using System.Linq;
using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Core.Enums;
using RPGF.Core.Location;
using UnityEngine;

namespace RPGF.Overworld.Player
{
    public class PlayerOverworldManager : RPGFrameworkBehaviour
    {
        public PlayerOverworldMovement movement;
        public PlayerOverworldInteraction interaction;

        public bool IsJumping { get; private set; }

        public void TeleportToPoint(string pointname)
        {
            LocationSpawnPoint point = LocalManager.GetCurrentLocation().SpawnPoints.FirstOrDefault(i => i.Name == pointname);

            if (point == null)
            {
                Debug.LogError("Point not found");

                return;
            }

            transform.position = point.transform.position;

            Overworld.CharacterManager.RebuildModels();

            movement.RotateTo(point.SpawnDirection);
        }

        public void TeleportToVector(Vector2 position)
        {
            transform.position = position;
        }

        public void Jump(Vector2 jumpFrom, ViewDirection direction, float length, float speed, float arcHeight, Ease ease, Action onComplete = null)
        {
            if (IsJumping || movement.IsAutoMoving)
                return;

            IsJumping = true;

            movement.SetMovementAccess(false);
            interaction.CanInteract = false;

            movement.SnapTo(jumpFrom);
            Overworld.CharacterManager.SetOtherModelsInvisible(CharacterManager.DEFAULT_FADE_TIME);

            Vector2 offset = DirectionHelper.GetVectorByViewDiretion(direction) * length;
            float time = speed > 0f ? length / speed : 0f;

            movement.TranslateByParabola(offset, time, arcHeight, ease, () =>
            {
                Overworld.CharacterManager.ResetOthersPositions();
                Overworld.CharacterManager.SetOtherModelsVisible(CharacterManager.DEFAULT_FADE_TIME);

                movement.SetMovementAccess(true);
                interaction.CanInteract = true;
                IsJumping = false;

                onComplete?.Invoke();
            });
        }
    }
}