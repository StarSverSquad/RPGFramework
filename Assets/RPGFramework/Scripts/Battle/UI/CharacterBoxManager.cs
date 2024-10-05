using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBoxManager : MonoBehaviour, IDisposable, IActive
{
    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private CharacterBox[] boxes = new CharacterBox[4];
    public CharacterBox[] Boxes => boxes;

    public Vector2[] BoxesGlobalPositions => boxes.Select(i => (Vector2)i.transform.position).ToArray();

    public float TraslateContainerTime => 0.6f;
    public float TraslateBoxTime => 0.25f;

    private void Start()
    {
        Dispose();
    }

    public void Initialize(params BattleCharacterInfo[] characters)
    {
        SetActive(true);

        int count = Mathf.Min(characters.Length, boxes.Length);
        for (int i = 0; i < count; i++)
        {
            boxes[i].gameObject.SetActive(true);
            boxes[i].Initialize(characters[i]);
        }
    }

    public CharacterBox GetBox(BattleCharacterInfo character) => boxes.First(i => i.Character == character);

    public void Show()
    {
        container.DOAnchorPosY(105, TraslateContainerTime).SetEase(Ease.OutCirc).Play();
    }
    public void Hide()
    {
        container.DOAnchorPosY(-160, TraslateContainerTime).SetEase(Ease.InCirc).Play();
    }

    public void FocusBox(int index)
    {
        RectTransform rect = boxes[index].GetComponent<RectTransform>();

        rect.DOAnchorPosY(160, TraslateBoxTime).SetEase(Ease.Linear).Play();
    }
    public void UnfocusBox(int index)
    {
        RectTransform rect = boxes[index].GetComponent<RectTransform>();

        rect.DOAnchorPosY(108, TraslateBoxTime).SetEase(Ease.Linear).Play();
    }

    public void FocusBox(BattleCharacterInfo character)
    {
        RectTransform rect = boxes.First(i => i.Character == character).GetComponent<RectTransform>();

        rect.DOAnchorPosY(150, TraslateBoxTime).SetLoops(0).SetEase(Ease.OutSine).Play();
    }
    public void UnfocusBox(BattleCharacterInfo character)
    {
        RectTransform rect = boxes.First(i => i.Character == character).GetComponent<RectTransform>();

        rect.DOAnchorPosY(108, TraslateBoxTime).SetLoops(0).SetEase(Ease.OutSine).Play();
    }

    public void Dispose()
    {
        foreach (var item in boxes)
        {
            item.Dispose();
            item.gameObject.SetActive(false);
        }

        SetActive(false);
    }

    public void SetActive(bool active) => container.gameObject.SetActive(active);
}