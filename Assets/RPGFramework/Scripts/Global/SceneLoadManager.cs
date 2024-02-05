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

        GameManager.Instance.GameAudio.StopBGM(1f);
        GameManager.Instance.GameAudio.StopBGS(1f);

        GameManager.Instance.LoadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.LoadingScreen.BgIsFade);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        GameManager.Instance.LoadingScreen.ActivatePart2();

        while (!asyncOperation.isDone)
        {
            GameManager.Instance.LoadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        GameManager.Instance.LoadingScreen.DeactivatePart2();

        GameManager.Instance.LoadingScreen.DeactivatePart1();

        isLoading = false;

        LoadingComplete?.Invoke();
    }

    private IEnumerator LoadGameCoroutine(string scene)
    {
        isLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        GameManager.Instance.LoadingScreen.ActivatePart2();

        while (!asyncOperation.isDone || ExplorerManager.Instance == null)
        {
            GameManager.Instance.LoadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        GameManager.Instance.LoadingScreen.DeactivatePart2();

        isLoading = false;

        LoadingComplete?.Invoke();
    }
}
