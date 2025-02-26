using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttackQTE : MonoBehaviour
{
    [Header("Links")]
    [SerializeField]
    private RectTransform slider;
    [SerializeField]
    private RectTransform container;

    [SerializeField] 
    private Vector2 sliderRigthPosition;
    [SerializeField]
    private Vector2 sliderLeftPosition;

    [SerializeField]
    private Image sliderImage;

    [SerializeField]
    private QTEEffectLine qteEffectLine;

    [SerializeField]
    private Color[] sliderColors = new Color[3];

    [SerializeField]
    private float damageFactor = 0;
    public float DamageFactor => damageFactor;

    [Header("Data")]
    [SerializeField]
    private bool isWorking = false;
    public bool IsWorking => isWorking;

    [SerializeField]
    private bool isSkip = false;
    public bool IsSkip => isSkip;

    public AnimationCurve sliderCurve;
    public AnimationCurve damageCurve;

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

            damageFactor = 0;

            OnSkip?.Invoke();
            OnMiss?.Invoke();
        }
        else
        {
            OnHit?.Invoke();

            sliderImage.enabled = true;

            damageFactor = damageCurve.Evaluate(load);

            GameObject endSlide = Instantiate(qteEffectLine.gameObject, container);
            QTEEffectLine qTEEffect = endSlide.GetComponent<QTEEffectLine>();

            endSlide.transform.position = slider.position;

            if (damageFactor < 0.25f)
            {
                qTEEffect.lineColor = sliderColors[1];
                OnMiss?.Invoke();
            }
            else if (damageFactor < 1f)
            {
                qTEEffect.lineColor = sliderColors[0];
            }
            else
            {
                qTEEffect.lineColor = sliderColors[2];
                OnCrit?.Invoke();
            }

            qTEEffect.Invoke();
        }

        slider.anchoredPosition = sliderRigthPosition;

        isWorking = false;

        OnEnd?.Invoke();
    }
}
