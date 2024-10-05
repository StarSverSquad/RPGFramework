using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterQueryManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject ElementPrefab;

    [SerializeField]
    private List<CharacterQueryElement> elements = new List<CharacterQueryElement>();

    [SerializeField]
    private float offset;

    public void Show()
    {
        float currentOffset = offset;

        foreach (var character in BattleManager.Data.Characters.Skip(1))
        {
            GameObject instance = Instantiate(ElementPrefab, transform);

            var element = instance.GetComponent<CharacterQueryElement>();
            element.Initialize(character);

            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, currentOffset);

            elements.Add(element);

            currentOffset += rectTransform.sizeDelta.y + offset;
        }

        StartCoroutine(QueryCoroutine());

        content.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void Hide()
    {
        foreach (var item in elements)
        {
            item.StopAnimation();
            Destroy(item.gameObject);
        }
        elements.Clear();

        content.DOAnchorPosY(-content.sizeDelta.y, 0.5f).SetEase(Ease.OutCirc).Play();
    }

    public void NextCharacter()
    {
        foreach (var item in elements)
        {
            var rect = item.GetComponent<RectTransform>();

            rect.DOAnchorPosY(-(offset + rect.sizeDelta.y), 0.25f).SetRelative().SetEase(Ease.OutCirc).Play();
        }
    }

    public void PreviouslyCharacter()
    {
        foreach (var item in elements)
        {
            var rect = item.GetComponent<RectTransform>();

            rect.DOAnchorPosY(offset + rect.sizeDelta.y, 0.25f).SetRelative().SetEase(Ease.OutCirc).Play();
        }
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
}