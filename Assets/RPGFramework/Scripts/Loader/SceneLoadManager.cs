using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    private bool isLoading = false;
    public bool IsLoading => isLoading;

    public event Action LoadingComplete;

    public void LoadScene(string SceneName)
    {
        if (!isLoading)
            StartCoroutine(LoadingCoroutine(SceneName));
    }

    private IEnumerator LoadingCoroutine(string scene)
    {
        isLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        GameManager.Instance.loadingScreen.ActivatePart2();

        while (!asyncOperation.isDone || ExplorerManager.instance == null)
        {
            GameManager.Instance.loadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        GameManager.Instance.loadingScreen.DeactivatePart2();

        isLoading = false;

        LoadingComplete?.Invoke();
    }
}
