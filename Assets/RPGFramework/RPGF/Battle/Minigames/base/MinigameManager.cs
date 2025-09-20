using UnityEngine;

public class MinigameManager : RPGFrameworkBehaviour
{
    [SerializeField]
    private Transform container;

    private MinigameBase currentMinigame = null;
    public MinigameBase CurrentMinigame => currentMinigame;

    public bool MinigameIsPlay => currentMinigame != null;

    public float LastWinFactor { get; private set; } = 0f;

    public void InvokeMinigame(MinigameBase minigame)
    {
        if (currentMinigame != null)
        {
            Debug.LogWarning("Миниигра уже запущена!");
            return;
        }

        var obj = Instantiate(minigame.gameObject, container);

        currentMinigame = obj.GetComponent<MinigameBase>();
        currentMinigame.OnEnd += OnMinigameEnd;

        currentMinigame.Invoke();
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
        LastWinFactor = currentMinigame.WinFactor;

        currentMinigame.OnEnd -= OnMinigameEnd;

        Destroy(currentMinigame.gameObject);

        currentMinigame = null;
    }
}
