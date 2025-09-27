using RPGF.Core;
using RPGF.Core.Location;
using System.Linq;
using UnityEngine;

namespace RPGF.Explorer.Player
{
    public class PlayerExplorerManager : RPGFrameworkBehaviour
    {
        public PlayerExplorerMovement movement;
        public PlayerExplorerInteraction interaction;

        public void TeleportToPoint(string pointname)
        {
            LocationSpawnPoint point = LocalManager.GetCurrentLocation().SpawnPoints.FirstOrDefault(i => i.Name == pointname);

            if (point == null)
            {
                Debug.LogError("Точка не найдена");

                return;
            }

            transform.position = point.transform.position;

            LocalManager.Instance.Character.RebuildModels();

            movement.RotateTo(point.SpawnDirection);
        }

        public void TeleportToVector(Vector2 position)
        {
            transform.position = position;
        }
    }
}