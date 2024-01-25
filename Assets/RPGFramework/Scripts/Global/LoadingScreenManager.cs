using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject partOneContainer;
    [SerializeField]
    private GameObject partTwoContainer;

    [SerializeField]
    private Image bg;

    [SerializeField]
    private TextMeshProUGUI textLoadingIndicator;

    [SerializeField]
    private float loadingProgress = 0;
    public float LoadingProgress
    {
        get => loadingProgress;
        set => loadingProgress = value;
    }

    public float BgFadeTime = 0.5f;

    [SerializeField]
    private bool bgIsFade = false;
    public bool BgIsFade => bgIsFade;

    private void OnEnable()
    {
        partOneContainer.SetActive(false);
        partTwoContainer.SetActive(false);


    }

    private void Update()
    {
        if (partTwoContainer.activeSelf)
            textLoadingIndicator.text = $"Loading: {Mathf.Floor(loadingProgress * 100f)}%";
    }

    public void ActivatePart1()
    {
        partOneContainer.SetActive(true);

        StartCoroutine(BgFadingCoroutine(true));
    }

    public void DeactivatePart1()
    {
        StartCoroutine(BgFadingCoroutine(false));
    }

    public void ActivatePart2()
    {
        partTwoContainer.SetActive(true);
    }

    public void DeactivatePart2()
    {
        partTwoContainer.SetActive(false);
    }

    private IEnumerator BgFadingCoroutine(bool dir)
    {
        bgIsFade = true;

        Color bgcolor = bg.color;

        bgcolor.a = dir ? 0 : 1;
        bg.color = bgcolor;

        float alphatarget = dir ? 1 : 0;

        float speed = (alphatarget - bgcolor.a) / BgFadeTime;

        float time = BgFadeTime;

        while (time > 0)
        {
            yield return new WaitForFixedUpdate();

            bgcolor.a += speed * Time.fixedDeltaTime;

            bg.color = bgcolor;

            time -= Time.fixedDeltaTime;
        }

        if (!dir)
            partOneContainer.SetActive(false);

        bgIsFade = false;
    }
}
