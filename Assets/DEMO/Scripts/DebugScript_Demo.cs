using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript_Demo : MonoBehaviour
{
    public LocationInfo Location;

    public RPGCharacter Character;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            GameManager.Instance.SaveLoad.Save(0);

        if (Input.GetKeyDown(KeyCode.L))
            GameManager.Instance.SaveLoad.Load(0);
    }
}
