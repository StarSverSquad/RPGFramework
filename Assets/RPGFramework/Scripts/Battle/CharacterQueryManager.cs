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

    private Queue<string> actions = new Queue<string>();

    private Tween contentTw;

    private Coroutine actionCorotine = null;
    private Coroutine updateCoroutine = null;

    public void Show()
    {
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

        updateCoroutine = StartCoroutine(UpdateCoroutine());

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

        StopAllCoroutines();

        contentTw = content.DOAnchorPosY(-content.sizeDelta.y, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void NextPosition()
    {
        actions.Enqueue("next");
    }

    public void PreviewPosition()
    {
        actions.Enqueue("preview");
    }

    private IEnumerator WaveCoroutine()
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

        actionCorotine = null;
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

        actionCorotine = null;
    }

    private IEnumerator UpdateCoroutine()
    {
        var prefabElement = ElementPrefab.GetComponent<CharacterQueryElement>();

        while (true)
        {
            if (actions.Count > 0)
            {
                if (actionCorotine != null)
                    StopCoroutine(actionCorotine);

                string act = actions.Dequeue();

                if (act == "next")
                {
                    var firstElement = queue.First();

                    queue.RemoveAt(0);
                    queue.Add(firstElement);

                    actionCorotine = StartCoroutine(NextCoroutine());
                }
                else
                {
                    var lastElement = queue.Last();

                    queue.RemoveAt(queue.Count - 1);
                    queue.Insert(0, lastElement);

                    actionCorotine = StartCoroutine(PreviewCoroutine());
                }

                yield return new WaitForSeconds(prefabElement.moveDuration);

                yield return new WaitWhile(() => actionCorotine != null);
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        contentTw.Kill();
    }
}