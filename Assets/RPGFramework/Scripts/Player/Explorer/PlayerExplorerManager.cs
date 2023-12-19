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
    /// ������������� �� ����� � �������
    /// </summary>
    /// <param name="pointname">�������� �����</param>
    public void TeleportToPoint(string pointname)
    {
        LocationSpawnPoint point = ExplorerManager.instance.locationManager.Current.SpawnPoints.FirstOrDefault(i => i.Name == pointname);

        if (point == null)
        {
            Debug.LogError("����� �� �������");

            return;
        }

        transform.position = point.transform.position;

        ExplorerManager.instance.characterManager.UpdateModels();

        movement.RotateTo(point.SpawnDirection);
    }

    public void TeleportToVector(Vector2 position)
    {
        transform.position = position;
    }
}