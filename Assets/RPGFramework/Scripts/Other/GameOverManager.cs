using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(5);

        GameManager.Instance.Character.Dispose();
        GameManager.Instance.Inventory.Dispose();

        GameManager.Instance.LocationManager.ChangeLocation(GameManager.Instance.LocationManager.CurrentLocation);
    }
}
