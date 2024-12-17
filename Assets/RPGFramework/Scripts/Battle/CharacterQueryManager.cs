using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterQueryManager : RPGFrameworkBehaviour
{
    [SerializeField]
    private RectTransform inSlot;
    [SerializeField]
    private RectTransform outSlot;
    [SerializeField]
    private RectTransform[] slots = new RectTransform[5];

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject ElementPrefab;

    [SerializeField]
    private List<CharacterQueryElement> elements = new List<CharacterQueryElement>();

    private List<CharacterQueryElement> queue = new List<CharacterQueryElement>();

    [SerializeField]
    private float offset;

    

    private int CurrentCharacterIndex => Battle.Pipeline.CurrentTurnDataIndex;

    private Tween contentTw;

    private Coroutine updatePisitionCoroutine = null;

    public void Show()
    {
        float currentOffset = offset;

        for (int i = 0; i < BattleManager.Data.TurnsData.Count; i++)
        {
            var turnsData = BattleManager.Data.TurnsData[i];

            GameObject instance = Instantiate(ElementPrefab, transform);

            var element = instance.GetComponent<CharacterQueryElement>();
            element.Initialize(turnsData.Character);

            var rectTransform = instance.GetComponent<RectTransform>();

            if (i < 4)
            {
                rectTransform.anchoredPosition = slots[i].anchoredPosition;
            }
            else
            {
                rectTransform.anchoredPosition = inSlot.anchoredPosition;
            }

            elements.Add(element);
            queue.Add(element);
        }

        contentTw = content.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void Hide()
    {
        foreach (var item in elements)
        {
            item.GetComponent<RectTransform>().DOKill();
            //item.StopAnimation();
            Destroy(item.gameObject);
        }
        elements.Clear();
        queue.Clear();

        contentTw = content.DOAnchorPosY(-content.sizeDelta.y, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void NextPosition()
    {
        var firstElement = queue.First();

        queue.RemoveAt(0);
        queue.Add(firstElement);

        if (updatePisitionCoroutine != null)
            StopCoroutine(updatePisitionCoroutine);

        updatePisitionCoroutine = StartCoroutine(NextCoroutine());
    }

    public void PreviewPosition()
    {
        var lastElement = queue.Last();

        queue.RemoveAt(queue.Count - 1);
        queue.Insert(0, lastElement);

        if (updatePisitionCoroutine != null)
            StopCoroutine(updatePisitionCoroutine);

        updatePisitionCoroutine = StartCoroutine(PreviewCoroutine());
    }

    private IEnumerator QueryCoroutine()
    {
        yield return new WaitForSeconds(.5f);

        foreach (var item in elements)
        {
            item.StartAnimation();

            yield return new WaitForSeconds(.25f);
        }
    }

    private IEnumerator NextCoroutine()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            var element = queue[i];

            if (i < slots.Length - 1)
            {
                bool isLast = i == queue.Count - 1;

                element.MoveToPoint(slots[i].anchoredPosition, 
                                   isLast ? slots.Last().anchoredPosition : slots[i + 1].anchoredPosition);
            }

            yield return new WaitForFixedUpdate();
        }

        updatePisitionCoroutine = null;
    }

    private IEnumerator PreviewCoroutine()
    {
        for (int i = queue.Count - 1; i >= 0; i--)
        {
            var element = queue[i];

            if (i < slots.Length - 1)
            {
                bool isFirst = i == 0;

                if (isFirst)
                {
                    element.MoveToPoint(slots.Last().anchoredPosition, element.GetComponent<RectTransform>().anchoredPosition);
                }
                else
                {
                    element.MoveToPoint(slots[i].anchoredPosition, slots[i - 1].anchoredPosition);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        updatePisitionCoroutine = null;
    }

    private void OnDestroy()
    {
        contentTw.Kill();
    }
}