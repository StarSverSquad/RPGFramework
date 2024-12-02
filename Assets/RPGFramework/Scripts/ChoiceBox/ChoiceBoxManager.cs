using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBoxManager : ChoiceBase<string>
{
    public enum Position
    {
        Top, Center, Bottom, Custom
    }

    /// <summary>
    /// Top, Center, Bottom
    /// </summary>
    [SerializeField]
    private RectTransform[] points = new RectTransform[3];

    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private ChoiceArrow leftArrow;
    [SerializeField]
    private ChoiceArrow rightArrow;

    private List<ChoiceItem> items = new List<ChoiceItem>();

    [SerializeField]
    private Vector2 Margin;

    [SerializeField]
    private float InBattleOffset;

    [SerializeField]
    private RectTransform choiceBox;
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject choiceItemPrefab;

    private Coroutine animatedTranslate = null;

    public void ChangePosition(Position position)
    {
        switch (position)
        {
            case Position.Top:
                choiceBox.anchoredPosition = points[0].anchoredPosition;
                break;
            case Position.Bottom:
                choiceBox.anchoredPosition = points[2].anchoredPosition;
                break;
            case Position.Center:
                choiceBox.anchoredPosition = points[1].anchoredPosition;
                break;
            default:
                choiceBox.anchoredPosition = Vector2.zero;
                break;
        }
    }
    public void ChangePosition(Position position, Vector2 cpos)
    {
        switch (position)
        {
            case Position.Top:
                choiceBox.anchoredPosition = points[0].anchoredPosition;
                break;
            case Position.Bottom:
                choiceBox.anchoredPosition = points[2].anchoredPosition;
                break;
            case Position.Center:
                choiceBox.anchoredPosition = points[1].anchoredPosition;
                break;
            case Position.Custom:
                choiceBox.anchoredPosition = cpos;
                break;
            default:
                choiceBox.anchoredPosition = Vector2.zero;
                break;
        }
    }

    public override void OnStart()
    {
        StartCoroutine(InitializeCoroutine());
    }

    public override void OnEnd()
    {
        choiceBox.gameObject.SetActive(false);

        if (animatedTranslate != null)
            StopCoroutine(animatedTranslate);

        animatedTranslate = null;

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();
    }

    public override void OnSellectChanged()
    {
        if (index == 0)
            leftArrow.gameObject.SetActive(false);
        else
            leftArrow.gameObject.SetActive(true);

        if (index == choices.Count - 1)
            rightArrow.gameObject.SetActive(false);
        else
            rightArrow.gameObject.SetActive(true);

        if (animatedTranslate != null)
            StopCoroutine(animatedTranslate);

        animatedTranslate = StartCoroutine(TranslateAnimation());
    }

    public override bool ConfirmCanExecuted()
    {
        return Input.GetKeyDown(GameManager.Instance.BaseOptions.Accept);
    }

    public override int SellectionChanging()
    {
        if (Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveRight))
        {
            rightArrow.Shake();

            return 1;
        }           
        else if (Input.GetKeyDown(GameManager.Instance.BaseOptions.MoveLeft))
        {
            leftArrow.Shake();

            return -1;
        }

        return 0;
    }

    private IEnumerator TranslateAnimation()
    {
        float offsetx = 0;
        for (int i = 0; i < index; i++)
            offsetx += items[i].XSize;

        float offsetdif = (-offsetx) - content.anchoredPosition.x;
        float offsetTime = Mathf.Abs(offsetdif / animationSpeed);

        float sizedif = items[index].XSize - choiceBox.sizeDelta.x;
        float sizeTime = Mathf.Abs(sizedif / animationSpeed);

        float maxTime = Mathf.Max(offsetTime, sizeTime);

        float cursize = choiceBox.sizeDelta.x;
        float curoffset = content.anchoredPosition.x;

        while (maxTime > 0)
        {
            yield return new WaitForFixedUpdate();

            curoffset = Mathf.Lerp(curoffset, -offsetx - Margin.x, animationSpeed * Time.fixedDeltaTime);
            cursize = Mathf.Lerp(cursize, items[index].XSize + Margin.y, animationSpeed * Time.fixedDeltaTime);

            content.anchoredPosition = new Vector2(curoffset, 0);
            choiceBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cursize);

            maxTime -= Time.fixedDeltaTime;
        }

        animatedTranslate = null;
    }

    private IEnumerator InitializeCoroutine()
    {
        float offsetx = 0;

        List<ChoiceItem> items = new List<ChoiceItem>();

        foreach (string choice in choices)
        {
            GameObject ci = Instantiate(choiceItemPrefab, content.position, Quaternion.identity, content);

            RectTransform itemtrans = ci.GetComponent<RectTransform>();

            itemtrans.anchoredPosition = new Vector2(offsetx, 0);

            ChoiceItem choiceitem = ci.GetComponent<ChoiceItem>();

            choiceitem.Initialize();

            choiceitem.Title = choice;

            items.Add(choiceitem);
        }

        yield return null;

        foreach (ChoiceItem item in items)
        {
            offsetx += item.XSize;

            items.Add(item);
        }

        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, offsetx);
        choiceBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, offsetx);

        OnSellectChanged();

        choiceBox.gameObject.SetActive(true);
    }
}
