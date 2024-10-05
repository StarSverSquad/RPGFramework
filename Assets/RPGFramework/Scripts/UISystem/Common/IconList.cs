using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IconList : MonoBehaviour, IDisposable
{
    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    RectTransform content;

    [SerializeField]
    private List<Image> icons = new List<Image>();

    public Vector4 Margin;

    public bool SingleRow;
    public bool Slide;

    public float SlideSpeed;
    public float SlideDelay;

    public float NotSlideSize = 100f;

    public int ElementsPerRow;

    private Vector2 offset;

    public bool HasIcons => icons.Count > 0;

    private Coroutine slideCoroutine;

    public void UpdateIcons(params Sprite[] sprites)
    {
        offset.x = Margin.x;
        offset.y = Margin.y;

        foreach (var item in icons)
            Destroy(item.gameObject);
        icons.Clear();

        int rowel = 0;

        foreach (var item in sprites)
        {
            GameObject inst = Instantiate(iconPrefab, content, false);

            RectTransform rect = inst.GetComponent<RectTransform>();
            Image image = inst.GetComponent<Image>();

            image.sprite = item;

            if (SingleRow)
            {
                rect.anchoredPosition = new Vector2(offset.x, -Margin.y);

                offset.x += Margin.x / 2 + rect.sizeDelta.x;
            }
            else
            {
                rect.anchoredPosition = new Vector2(offset.x + Margin.x, -Margin.y * (rowel + 1) + (rect.sizeDelta.y * rowel));

                offset.x += Margin.x / 2 + rect.sizeDelta.x;

                rowel = rowel > ElementsPerRow - 1 ? 0 : rowel + 1;
            }

            icons.Add(image);
        }

        offset.x += Margin.z;
        offset.y += Margin.w;

        if (Slide && offset.x > NotSlideSize)
            slideCoroutine = StartCoroutine(SlideCoroutine());
        else if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);
    }

    private IEnumerator SlideCoroutine()
    {
        float dif = offset.x - NotSlideSize;

        float start = content.anchoredPosition.x;
        float end = content.anchoredPosition.x - dif;

        float slideTime = dif / SlideSpeed;
        float slideActualTime = slideTime;

        bool direction = true;

        yield return new WaitForSeconds(SlideDelay);

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (direction)
                content.anchoredPosition = new Vector2(Mathf.MoveTowards(content.anchoredPosition.x, end, SlideSpeed * Time.fixedDeltaTime),
                                                        content.anchoredPosition.y);
            else
                content.anchoredPosition = new Vector2(Mathf.MoveTowards(content.anchoredPosition.x, start, SlideSpeed * Time.fixedDeltaTime),
                                                            content.anchoredPosition.y);

            slideActualTime -= Time.fixedDeltaTime;

            if (slideActualTime <= 0)
            {
                yield return new WaitForSeconds(SlideDelay);

                slideActualTime = slideTime;

                direction = !direction;

                continue;
            }
        }
    }

    public void Dispose()
    {
        foreach (var item in icons)
            Destroy(item.gameObject);
        icons.Clear();
    }
}