using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackQTE : MonoBehaviour
{
    [Header("Links")]
    [SerializeField]
    private RectTransform slider;
    [SerializeField] 
    private RectTransform sliderStartPoint;
    [SerializeField]
    private RectTransform sliderEndPoint;
    [SerializeField]
    private Image sliderImage;

    [SerializeField]
    private Sprite[] sliderSprites = new Sprite[3];

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

    [Header("Options")]
    public float sliderTime;

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

        slider.anchoredPosition = sliderStartPoint.anchoredPosition;
        sliderImage.sprite = sliderSprites[0];

        bool slide = true;

        float load = 0;
        float loadspeed = 1 / sliderTime;

        Coroutine slidecor = StartCoroutine(Anims.MoveToByTime(sliderStartPoint.anchoredPosition.x, sliderEndPoint.anchoredPosition.x, sliderTime, val =>
        {
            slider.anchoredPosition = new Vector2(val, slider.anchoredPosition.y);

            load += loadspeed * Time.fixedDeltaTime;

        }, () => slide = false));

        bool click = false;

        while (!click && slide)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(slidecor);
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
                sliderImage.sprite = sliderSprites[1];
                damageFactor = MissFactor;

                OnMiss?.Invoke();
            }
            else if (load >= MissBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = MissFactor;

                OnMiss?.Invoke();
            }
            else if (load >= SecoundVeryBadBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = VeryBadFactor;
            }
            else if (load >= SecoundGoodBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = GoodFactor;
            }
            else if (load >= CritBorder)
            {
                sliderImage.sprite = sliderSprites[2];
                damageFactor = CritFactor;

                OnCrit?.Invoke();
            }
            else if (load >= FirstGoodBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = GoodFactor;
            }
            else if (load >= MediumBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = MediumFactor;
            }
            else if (load >= BadBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = BadFactor;
            }
            else if (load >= FirstVeryBadBorder)
            {
                sliderImage.sprite = sliderSprites[1];
                damageFactor = VeryBadFactor;
            }
        }

        isWorking = false;

        OnEnd?.Invoke();
    }
}
