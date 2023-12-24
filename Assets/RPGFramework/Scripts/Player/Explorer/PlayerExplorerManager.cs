using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlayerExplorerManager : MonoBehaviour
{
    public PlayerExplorerMovement movement;
    public PlayerExplorerInteraction interaction;


    /// <summary>
    /// Телепортирует на точку в локации
    /// </summary>
    /// <param name="pointname">Название точки</param>
    public void TeleportToPoint(string pointname)
    {
        LocationSpawnPoint point = LocalManager.GetCurrentLocation().SpawnPoints.FirstOrDefault(i => i.Name == pointname);

        if (point == null)
        {
            Debug.LogError("Точка не найдена");

            return;
        }

        transform.position = point.transform.position;

        LocalManager.Instance.Character.UpdateModels();

        movement.RotateTo(point.SpawnDirection);
    }

    public void TeleportToVector(Vector2 position)
    {
        transform.position = position;
    }
}
