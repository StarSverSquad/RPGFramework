using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterQueryManager : RPGFrameworkBehaviour, IDisposable
{
    [SerializeField]
    private RectTransform _inSlot;
    [SerializeField]
    private RectTransform _outSlot;
    [SerializeField]
    private RectTransform[] _slots = new RectTransform[5];

    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private GameObject _elementPrefab;

    public bool IsUpdating => updateCoroutine != null;
    public bool ActionIsPlay => actionCorotine != null;

    private List<CharacterQueryElement> elements = new();
    private List<CharacterQueryElement> queue = new();

    private Queue<string> actions = new();

    private Tween contentTw;

    private Coroutine actionCorotine = null;
    private Coroutine updateCoroutine = null;

    #region API

    public void Show()
    {
        Dispose();

        for (int i = 0; i < BattleManager.Data.TurnsData.Count; i++)
        {
            var turnsData = BattleManager.Data.TurnsData[i];

            GameObject instance = Instantiate(_elementPrefab, transform);

            var element = instance.GetComponent<CharacterQueryElement>();
            element.Initialize(turnsData.Character);

            var rectTransform = instance.GetComponent<RectTransform>();

            if (i < 4)
            {
                rectTransform.anchoredPosition = _slots[i].anchoredPosition;
            }
            else
            {
                rectTransform.anchoredPosition = _inSlot.anchoredPosition;
            }

            elements.Add(element);
            queue.Add(element);
        }

        updateCoroutine = StartCoroutine(UpdateCoroutine());

        contentTw = _content.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutCirc).Play();
    }
    public void Hide()
    {
        Dispose();

        contentTw = _content.DOAnchorPosY(-_content.sizeDelta.y, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void NextPosition()
    {
        UpdateDynamics();

        actions.Enqueue("next");
    }
    public void PreviewPosition()
    {
        UpdateDynamics();

        actions.Enqueue("preview");
    }

    #endregion

    private void UpdateDynamics()
    {
        foreach (var item in elements)
        {
            item.UpdateDynamic();
        }
    }

    private IEnumerator NextCoroutine()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            var element = queue[i];

            if (i < _slots.Length - 1)
            {
                bool isLast = i == queue.Count - 1;

                element.MoveToPoint(_slots[i].anchoredPosition, 
                                   isLast ? _slots.Last().anchoredPosition : _slots[i + 1].anchoredPosition);
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

            if (i < _slots.Length - 1)
            {
                bool isFirst = i == 0;

                if (isFirst)
                {
                    element.MoveToPoint(_slots.Last().anchoredPosition, element.GetComponent<RectTransform>().anchoredPosition);
                }
                else
                {
                    element.MoveToPoint(_slots[i].anchoredPosition, _slots[i - 1].anchoredPosition);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        actionCorotine = null;
    }

    private IEnumerator UpdateCoroutine()
    {
        var prefabElement = _elementPrefab.GetComponent<CharacterQueryElement>();

        while (Battle.Pipeline.MainIsWorking)
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

        updateCoroutine = null;
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        contentTw?.Kill();

        StopAllCoroutines();

        updateCoroutine = null;
        actionCorotine = null;

        foreach (var item in elements)
        {
            item.GetComponent<RectTransform>().DOKill();
            Destroy(item.gameObject);
        }
        elements.Clear();

        queue.Clear();

        actions.Clear();
    }
}