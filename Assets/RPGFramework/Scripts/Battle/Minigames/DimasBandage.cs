using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DimasBandage : MinigameBase
{
    private float progress = 0;

    public float ProgressDiv = 0.001f;
    public float ProgressAdd = 0.1f;

    public float GameTime = 5f;

    public float MinimumProgressBorder = 0.25f;

    [SerializeField]
    private Image bar;
    private float barMaxWidth = 0;

    [SerializeField]
    private Canvas canvas;

    private bool gameStarted = false;

    protected override IEnumerator Minigame()
    {
        barMaxWidth = bar.rectTransform.sizeDelta.x;

        //canvas. = Camera.main;

        gameStarted = true;

        yield return new WaitForSeconds(GameTime);

        gameStarted = false;

        if (progress < MinimumProgressBorder)
            progress = 0;
        else if (progress > 0.9f)
            progress = 1.25f;

        SetWinFactor(progress);

        yield return new WaitForSeconds(1f);
    }

    private void Update()
    {
        if (!gameStarted)
            return;

        if (Input.GetKeyDown(GameManager.Instance.BaseOptions.Accept))
        {
            progress += ProgressAdd;
        }

        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barMaxWidth * progress);

        bar.color = new Color(255 * (1 - progress), 0, 255 * progress);
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        progress -= ProgressDiv;
    }
}
