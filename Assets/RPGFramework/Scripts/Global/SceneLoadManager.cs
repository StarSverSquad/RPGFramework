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
            StartCoroutine(LoadCoroutine(SceneName));
    }

    public void LoadGameScene(string SceneName)
    {
        if (!isLoading)
            StartCoroutine(LoadGameCoroutine(SceneName));
    }

    private IEnumerator LoadCoroutine(string scene)
    {
        isLoading = true;

        GameManager.Instance.gameAudio.StopBGM(1f);
        GameManager.Instance.gameAudio.StopBGS(1f);

        GameManager.Instance.loadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        GameManager.Instance.loadingScreen.ActivatePart2();

        while (!asyncOperation.isDone)
        {
            GameManager.Instance.loadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        GameManager.Instance.loadingScreen.DeactivatePart2();

        GameManager.Instance.loadingScreen.DeactivatePart1();

        isLoading = false;

        LoadingComplete?.Invoke();
    }

    private IEnumerator LoadGameCoroutine(string scene)
    {
        isLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        GameManager.Instance.loadingScreen.ActivatePart2();

        while (!asyncOperation.isDone || ExplorerManager.Instance == null)
        {
            GameManager.Instance.loadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        GameManager.Instance.loadingScreen.DeactivatePart2();

        isLoading = false;

        LoadingComplete?.Invoke();
    }
}
