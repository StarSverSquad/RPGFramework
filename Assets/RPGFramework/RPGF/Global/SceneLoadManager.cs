using RPGF.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGF
{
    public class SceneLoadManager : RPGFrameworkBehaviour
    {
        public bool IsLoading => loadCoroutine != null;

        public event Action OnLoadingComplete;
        public event Action OnLoadingStart;
        public event Action<float> OnLoadingProgress;

        private Coroutine loadCoroutine = null;

        public void LoadScene(string SceneName)
        {
            if (!IsLoading)
                loadCoroutine = StartCoroutine(LoadCoroutine(SceneName));
        }

        private IEnumerator LoadCoroutine(string scene)
        {
            OnLoadingStart?.Invoke();

            var asyncOperation = SceneManager.LoadSceneAsync(scene);

            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                OnLoadingProgress?.Invoke(asyncOperation.progress);

                yield return null;
            }

            loadCoroutine = null;

            OnLoadingComplete?.Invoke();
        }
    }
}
