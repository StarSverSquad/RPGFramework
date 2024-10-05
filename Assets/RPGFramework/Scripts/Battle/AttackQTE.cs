using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class AttackQTE : MonoBehaviour
{
    [Header("Links")]
    [SerializeField]
    private RectTransform slider;
    [SerializeField] 
    private Vector2 sliderRigthPosition;
    [SerializeField]
    private Vector2 sliderLeftPosition;
    [SerializeField]
    private Image sliderImage;

    [SerializeField]
    private Color[] sliderColors = new Color[3];

    [SerializeField]
    private float damageFactor = 0;
    public float DamageFactor => damageFactor;

    [Header("Factors")]
    public float MissFactor = 0;
    public float VeryBadFactor = 0.25f;
    public float BadFactor = 0.5f;
    public float MediumFactor = 0.75f;
    public float GoodFactor = 1f;
    public float CritFactor = 1.25f;

    [Header("Data")]
    [SerializeField]
    private bool isWorking = false;
    public bool IsWorking => isWorking;

    [SerializeField]
    private bool isSkip = false;
    public bool IsSkip => isSkip;

    public AnimationCurve sliderCurve;

    [Header("Borders")]
    [Range(0f, 1f)]
    public float FirstVeryBadBorder = 0;
    [Range(0f, 1f)]
    public float BadBorder = 0;
    [Range(0f, 1f)]
    public float MediumBorder = 0;
    [Range(0f, 1f)]
    public float FirstGoodBorder = 0;
    [Range(0f, 1f)]
    public float CritBorder = 0;
    [Range(0f, 1f)]
    public float SecoundGoodBorder = 0;
    [Range(0f, 1f)]
    public float SecoundVeryBadBorder = 0;
    [Range(0f, 1f)]
    public float MissBorder = 0;

    public event Action OnHit;
    public event Action OnMiss;
    public event Action OnSkip;
    public event Action OnCrit;
    public event Action OnStart;
    public event Action OnEnd;

    public void Invoke()
    {
        if (!isWorking)
        {
            StartCoroutine(QTE());
        }
    }

    private IEnumerator QTE()
    {
        isWorking = true;

        OnStart?.Invoke();

        isSkip = false;

        slider.anchoredPosition = sliderRigthPosition;
        sliderImage.color = sliderColors[0];

        bool slide = true;

        float load = 0;

        Coroutine slideCoroutine = StartCoroutine(AnimationPack.MoveByCurve(sliderCurve, value =>
        {
            slider.anchoredPosition = new Vector2(sliderRigthPosition.x + (sliderLeftPosition.x - sliderRigthPosition.x) * value, slider.anchoredPosition.y);

            load = value;
        }, () => slide = false));

        bool click = false; 

        while (!click && slide)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(slideCoroutine);
                click = true;
            }
        }

        if (!click)
        {
            isSkip = true;

            damageFactor = MissFactor;

            OnSkip?.Invoke();
        }
        else
        {
            OnHit?.Invoke();

            sliderImage.enabled = true;

            if (load < FirstVeryBadBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = MissFactor;

                OnMiss?.Invoke();
            }
            else if (load >= MissBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = MissFactor;

                OnMiss?.Invoke();
            }
            else if (load >= SecoundVeryBadBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = VeryBadFactor;
            }
            else if (load >= SecoundGoodBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = GoodFactor;
            }
            else if (load >= CritBorder)
            {
                sliderImage.color = sliderColors[2];
                damageFactor = CritFactor;

                OnCrit?.Invoke();
            }
            else if (load >= FirstGoodBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = GoodFactor;
            }
            else if (load >= MediumBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = MediumFactor;
            }
            else if (load >= BadBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = BadFactor;
            }
            else if (load >= FirstVeryBadBorder)
            {
                sliderImage.color = sliderColors[1];
                damageFactor = VeryBadFactor;
            }
        }

        slider.anchoredPosition = sliderRigthPosition;

        isWorking = false;

        OnEnd?.Invoke();
    }
}
