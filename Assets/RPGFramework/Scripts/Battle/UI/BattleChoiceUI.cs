using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleChoiceUI : CommonChoiceUI
{
    [Header("Scroll settings")]
    [SerializeField]
    private bool canScroll = false;
    public bool CanScroll => canScroll;
    [SerializeField]
    private float scrollSpeed = 4f;
    public float ScrollSpeed => scrollSpeed;

    private Coroutine scrollCoroutine = null;

    private Tween scrollTween;
    private Dictionary<ElementInfo, float> elementsDefaultAbsY;

    public bool IsCrolling => scrollCoroutine != null;

    private void Start()
    {
        OnStart += BattleChoiceUI_OnStartChoice;
        OnEnd += BattleChoiceUI_OnEndChoice;
        OnSellectionChanged += BattleChoiceUI_OnSellectionChanged;

        CleanUp();
    }

    private void BattleChoiceUI_OnSellectionChanged()
    {
        if (!canScroll)
            return;

        scrollTween.Kill();

        Vector3[] rectCorners = new Vector3[4];
        Vector3[] itemCorners = new Vector3[4];
        Vector3[] contentCorners = new Vector3[4];

        rect.GetWorldCorners(rectCorners);
        CurrentItem.element.GetComponent<RectTransform>().GetWorldCorners(itemCorners);
        content.GetWorldCorners(contentCorners);

        float contentToElementTop = Mathf.Abs(itemCorners[1].y - contentCorners[1].y);
        float contentToElementBottom = Mathf.Abs(itemCorners[0].y - contentCorners[0].y);

        if (itemCorners[1].y > rectCorners[1].y)
        {
            scrollTween = content.DOMoveY(content.position.y - Mathf.Abs(itemCorners[1].y - rectCorners[1].y) - Margin.y / 48, 0.25f).SetEase(Ease.OutQuint).Play();
        }

        if (itemCorners[0].y < rectCorners[0].y)
        {
            scrollTween = content.DOMoveY(content.position.y + Mathf.Abs(itemCorners[0].y - rectCorners[0].y) + Margin.w / 48, 0.25f).SetEase(Ease.OutQuint).Play();
        }
    }

    private void OnDestroy()
    {
        OnStart -= BattleChoiceUI_OnStartChoice;
        OnEnd -= BattleChoiceUI_OnEndChoice;
    }

    private void BattleChoiceUI_OnStartChoice()
    {
        elementsDefaultAbsY = new Dictionary<ElementInfo, float>();

        foreach (var lst in elementLists)
        {
            foreach (var item in lst)
            {
                elementsDefaultAbsY.Add(item, item.element.transform.position.y);
            }
        }
    }

    private void BattleChoiceUI_OnEndChoice()
    {
        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);

        scrollCoroutine = null;
    }

    public override void CleanUp()
    {
        base.CleanUp();

        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);
    }
}