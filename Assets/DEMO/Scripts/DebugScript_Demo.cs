using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript_Demo : MonoBehaviour
{
    public LocationInfo Location;

    public RPGCharacter Character;

    public RPGCollectable Item0;
    public RPGCollectable Item1;
    public RPGCollectable Item2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GameManager.Instance.locationManager.ChangeLocation(Location);

        if (Input.GetKeyDown(KeyCode.S))
            GameManager.Instance.saveLoad.Save(1);

        if (Input.GetKeyDown(KeyCode.L))
            GameManager.Instance.saveLoad.Load(1);

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.character.AddCharacter(Character);
            LocalManager.Instance.Character.UpdateModels();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.inventory.AddToItemCount(Item0, 1);
            GameManager.Instance.inventory.AddToItemCount(Item1, 2);
            GameManager.Instance.inventory.AddToItemCount(Item2, 3);
        }
    }
}
