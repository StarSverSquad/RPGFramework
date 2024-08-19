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

    public bool IsCrolling => scrollCoroutine != null;

    private void Start()
    {
        OnStart += BattleChoiceUI_OnStartChoice;
        OnEnd += BattleChoiceUI_OnEndChoice;

        CleanUp();
    }

    private void OnDestroy()
    {
        OnStart -= BattleChoiceUI_OnStartChoice;
        OnEnd -= BattleChoiceUI_OnEndChoice;
    }

    private void BattleChoiceUI_OnStartChoice()
    {
        scrollCoroutine = StartCoroutine(ScrollCoroutine());
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

    private IEnumerator ScrollCoroutine()
    {
        if (!canScroll)
        {
            scrollCoroutine = null;
            yield break;
        }

        Vector3[] mainCornets = new Vector3[4];
        Vector3[] contentCornets = new Vector3[4];
        Vector3[] itemCornets = new Vector3[4];

        rect.GetWorldCorners(mainCornets);
        elementPrefab.GetComponent<RectTransform>().GetWorldCorners(itemCornets);

        float elOffset = (itemCornets[0].y - itemCornets[1].y) / 2f;

        Vector2 mltop = mainCornets[1];
        Vector2 mlbottom = mainCornets[0];

        Vector2 mlcenter = mltop + ((mlbottom - mltop) / 2);

        while (true)
        {
            yield return new WaitForFixedUpdate();

            content.GetWorldCorners(contentCornets);

            Vector2 cltop = contentCornets[1];
            Vector2 clbottom = contentCornets[0];
            Vector2 cldif = clbottom - cltop;

            Vector2 offset = CurrentItem.element.transform.position - content.transform.position;

            Vector2 result = content.transform.position;

            if (cldif.magnitude <= (mlbottom - mltop).magnitude)
                break;

            if (canScroll)
            {
                result.y = Mathf.Lerp(content.transform.position.y, mlcenter.y - offset.y - elOffset, scrollSpeed * Time.fixedDeltaTime);

                if (result.y < mltop.y)
                    result.y = Mathf.MoveTowards(content.transform.position.y, mltop.y, scrollSpeed * Time.fixedDeltaTime);
                else if (result.y + cldif.y > mlbottom.y)
                    result.y = Mathf.MoveTowards(content.transform.position.y, mlbottom.y - cldif.y, scrollSpeed * Time.fixedDeltaTime);
            }

            content.transform.position = result;
        }

        scrollCoroutine = null;
    }
}