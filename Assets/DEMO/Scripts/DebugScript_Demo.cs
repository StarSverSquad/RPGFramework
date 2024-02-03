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
            GameManager.Instance.locationManager.ChangeLocation(Location);

        if (Input.GetKeyDown(KeyCode.S))
            GameManager.Instance.saveLoad.Save(1);

        if (Input.GetKeyDown(KeyCode.L))
            GameManager.Instance.saveLoad.Load(1);

        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.character.Characters[0].Heal -= 25;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.character.Characters[0].Expireance = GameManager.Instance.character.Characters[0].ExpireanceBorder;

            GameManager.Instance.character.Characters[0].LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.character.AddCharacter(Character);
            LocalManager.Instance.Character.UpdateModels();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (var item in GameManager.Instance.GameData.Collectables)
            {
                GameManager.Instance.inventory.AddToItemCount(item, 1);
            } 
        }
    }
}
