using UnityEngine;

public class MinigameManager
{
    private MinigameBase currentMinigame = null;
    public MinigameBase CurrentMinigame => currentMinigame;

    public void InvokeMinigame(MinigameBase minigame)
    {
        if (currentMinigame != null)
        {
            Debug.LogWarning("Миниигра уже запущена!");
            return;
        }

        currentMinigame = minigame;
        currentMinigame.OnEnd += OnMinigameEnd;

        minigame.Invoke();
    }

    public void CancelMinigame()
    {
        if (currentMinigame == null)
        {
            Debug.LogWarning("Миниигра не запущена!");
            return;
        }

        currentMinigame.StopAllCoroutines();

        OnMinigameEnd();
    }

    private void OnMinigameEnd()
    {
        currentMinigame.OnEnd -= OnMinigameEnd;

        Object.Destroy(currentMinigame.gameObject);

        currentMinigame = null;
    }
}
