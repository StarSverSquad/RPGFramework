using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : RPGFrameworkBehaviour
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

        Game.GameAudio.StopBGM(1f);
        Game.GameAudio.StopBGS(1f);

        Game.LoadingScreen.ActivatePart1();

        yield return new WaitWhile(() => Game.LoadingScreen.BgIsFade);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        Game.LoadingScreen.ActivatePart2();

        while (!asyncOperation.isDone)
        {
            Game.LoadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        Game.LoadingScreen.DeactivatePart2();

        Game.LoadingScreen.DeactivatePart1();

        isLoading = false;

        LoadingComplete?.Invoke();
    }

    private IEnumerator LoadGameCoroutine(string scene)
    {
        isLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        asyncOperation.allowSceneActivation = true;

        Game.LoadingScreen.ActivatePart2();

        while (!asyncOperation.isDone || ExplorerManager.Instance == null)
        {
            Game.LoadingScreen.LoadingProgress = asyncOperation.progress;

            yield return null;
        }

        Game.LoadingScreen.DeactivatePart2();

        isLoading = false;

        LoadingComplete?.Invoke();
    }
}
