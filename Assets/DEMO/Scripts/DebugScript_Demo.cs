using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript_Demo : MonoBehaviour
{
    public LocationInfo Location;

    public RPGCharacter Character;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GameManager.Instance.LocationManager.ChangeLocation(Location);

        if (Input.GetKeyDown(KeyCode.S))
            GameManager.Instance.SaveLoad.Save(1);

        if (Input.GetKeyDown(KeyCode.L))
            GameManager.Instance.SaveLoad.Load(1);

        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.Character.Characters[0].Heal -= 25;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Character.Characters[0].Expireance = GameManager.Instance.Character.Characters[0].ExpireanceBorder;

            GameManager.Instance.Character.Characters[0].LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.Character.AddCharacter(Character);
            LocalManager.Instance.Character.UpdateModels();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (var item in GameManager.Instance.GameData.Collectables)
            {
                GameManager.Instance.Inventory.AddToItemCount(item, 1);
            } 
        }
    }
}
