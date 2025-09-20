using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MediaManager : MonoBehaviour, IManagerInitialize
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private GameObject container;

    private bool isFade = false;
    public bool IsFade => isFade;

    public void Initialize()
    {
        container.SetActive(false);
        isFade = false;
    }

    public void ShowImage(Sprite sprite, float fadeTime = 0)
    {
        FadeCheck();

        image.color = Color.white;
        image.sprite = sprite;

        container.SetActive(true);

        if (fadeTime > 0)
        {
            image.color = new Color(1, 1, 1, 0);

            isFade = true;
            image.DOFade(1, fadeTime).Play().onComplete = () =>
            {
                isFade = false;
            };
        }
    }

    public void ShowColor(Color color, float fadeTime = 0)
    {
        FadeCheck();

        image.sprite = null;

        container.SetActive(true);

        if (fadeTime > 0)
        {
            image.color = new Color(color.r, color.g, color.b, 0);

            isFade = true;
            image.DOFade(1, fadeTime).Play().onComplete = () =>
            {
                isFade = false;
            };
        }
        else
        {
            image.color = color;
        }
    }

    public void HideImage(float fadeTime = 0)
    {
        FadeCheck();

        if (fadeTime > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

            isFade = true;
            image.DOFade(1, fadeTime).Play().onComplete = () =>
            {
                container.SetActive(false);
                isFade = false;
            };
        }
        else
            container.SetActive(false);
    }

    private void FadeCheck()
    {
        if (isFade)
            image.DOKill(true);
    }

    private void OnDestroy()
    {
        image.DOKill();
    }
}