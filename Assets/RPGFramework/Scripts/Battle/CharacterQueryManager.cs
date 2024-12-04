using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterQueryManager : RPGFrameworkBehaviour
{
    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject ElementPrefab;

    [SerializeField]
    private List<CharacterQueryElement> elements = new List<CharacterQueryElement>();

    [SerializeField]
    private float offset;

    private int CurrentCharacterIndex => Battle.Pipeline.CurrentTurnDataIndex;

    private Tween contentTw;

    private Coroutine updatePisitionCoroutine = null;

    public void Show()
    {
        float currentOffset = offset;

        foreach (var turnsData in BattleManager.Data.TurnsData.Skip(1))
        {
            GameObject instance = Instantiate(ElementPrefab, transform);

            var element = instance.GetComponent<CharacterQueryElement>();
            element.Initialize(turnsData.Character);

            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, currentOffset);

            elements.Add(element);

            currentOffset += rectTransform.sizeDelta.y + offset;
        }

        StartCoroutine(QueryCoroutine());

        contentTw = content.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void Hide()
    {
        foreach (var item in elements)
        {
            item.GetComponent<RectTransform>().DOKill();
            item.StopAnimation();
            Destroy(item.gameObject);
        }
        elements.Clear();

        contentTw = content.DOAnchorPosY(-content.sizeDelta.y, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void UpdatePositions()
    {
        if (updatePisitionCoroutine != null)
            StopCoroutine(updatePisitionCoroutine);

        updatePisitionCoroutine = StartCoroutine(UpdatePisitionCoroutine());
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

    private IEnumerator NextCharacterCoroutine()
    {
        foreach (var item in elements)
        {
            var rect = item.GetComponent<RectTransform>();

            rect.DOKill();
            rect.DOAnchorPosY(-(offset + rect.sizeDelta.y), 0.25f).SetRelative().SetEase(Ease.OutCirc).Play();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator PreviouslyCharacterCoroutine()
    {
        foreach (var item in elements)
        {
            var rect = item.GetComponent<RectTransform>();

            rect.DOKill();
            rect.DOAnchorPosY(offset + rect.sizeDelta.y, 0.25f).SetRelative().SetEase(Ease.OutCirc).Play();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator UpdatePisitionCoroutine()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            var rect = elements[i].GetComponent<RectTransform>();

            rect.DOAnchorPosY(offset + ((rect.sizeDelta.y + offset) * i) - ((rect.sizeDelta.y + offset) * CurrentCharacterIndex), 0.25f)
                .SetEase(Ease.OutCirc).Play();

            yield return new WaitForSeconds(0.1f);
        }

        updatePisitionCoroutine = null;
    }

    private void OnDestroy()
    {
        contentTw.Kill();
    }
}