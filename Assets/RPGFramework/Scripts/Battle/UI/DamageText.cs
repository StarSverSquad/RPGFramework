using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private GameObject ftPrefab;

    [SerializeField]
    RectTransform rt;

    [SerializeField]
    private List<SimpleFallingText> fallingTexts = new List<SimpleFallingText>();
    [SerializeField]
    private List<string> letters = new List<string>();

    [SerializeField]
    private float letterOffset = 40f;
    [SerializeField]
    private float fallingTime = 0.1f;

    [SerializeField]
    private bool deleteOnEnd = false;
    [SerializeField]
    private float deletionDelay = 0.5f;

    [SerializeField]
    private bool isFalling = false;
    public bool IsFalling => isFalling;

    public void Invoke(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("Number can only more 0!");

            return;
        }

        foreach (var fall in fallingTexts)
            Destroy(fall.gameObject);
        fallingTexts.Clear();
        letters.Clear();

        float offset = 0;

        while (damage > 0)
        {
            int digit = damage % 10;
            damage /= 10;

            letters.Add(digit.ToString());

            offset += letterOffset;
        }

        rt.anchoredPosition += new Vector2(offset / 2, 0);

        for (int i = 0; i < letters.Count; i++)
        {
            GameObject n = Instantiate(ftPrefab, new Vector2(0, 0), Quaternion.identity, transform);

            fallingTexts.Add(n.GetComponent<SimpleFallingText>());
        }

        StartCoroutine(Animation());
    }

    public void OutputSimpleText(string text) => OutputSimpleText(text, Color.white, Color.white);
    public void OutputSimpleText(string text, Color cin, Color cout)
    {
        foreach (var fall in fallingTexts)
            Destroy(fall.gameObject);
        fallingTexts.Clear();
        letters.Clear();

        float offset = 0;

        foreach (var item in text.Reverse())
        {
            letters.Add(item.ToString());

            offset += letterOffset;
        }

        rt.anchoredPosition += new Vector2(offset / 2, 0);

        for (int i = 0; i < letters.Count; i++)
        {
            GameObject n = Instantiate(ftPrefab, new Vector2(0, 0), Quaternion.identity, transform);

            SimpleFallingText fallingText = n.GetComponent<SimpleFallingText>();
            fallingText.GradientIn = cin;
            fallingText.GradientOut = cout;

            fallingTexts.Add(fallingText);
        }

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        isFalling = true;

        float oneFallTime = fallingTime / letters.Count;

        float offset = 0;
        for (int i = 0; i < letters.Count; i++)
        {
            fallingTexts[i].FallingTime = oneFallTime;
            fallingTexts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 0);
            fallingTexts[i].Invoke(letters[i]);

            offset -= letterOffset;

            yield return new WaitForSeconds(oneFallTime / 2);
        }

        yield return new WaitWhile(() => fallingTexts.Last().IsFalling);

        isFalling = false;

        if (deleteOnEnd)
        {
            yield return new WaitForSeconds(deletionDelay);

            Destroy(gameObject);
        }
    }
}
