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

        GameManager.Instance.locationManager.ChangeLocation(GameManager.Instance.locationManager.CurrentLocation);
    }
}
