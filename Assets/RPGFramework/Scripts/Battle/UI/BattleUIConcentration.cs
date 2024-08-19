using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIConcentration : MonoBehaviour
{
    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private RectTransform barBg;
    [SerializeField]
    private RectTransform barFront;

    [SerializeField]
    private TextMeshProUGUI counter;

    public float ShowHideTime => 0.75f;
    public float UpperTime => 0.25f;

    public void SetConcentration(int value)
    {
        float ySize = (value / 100f) * barBg.sizeDelta.y;

        barFront.DOSizeDelta(new Vector2(barFront.sizeDelta.x, ySize), 0.15f).SetEase(Ease.OutCirc).Play();
        barFront.GetComponent<Image>().DOColor(new Color(.1f, 0.52f, 0), 0.4f)
            .From(new Color(.16f, 0.72f, 0.014f)).SetEase(Ease.OutCirc).Play();

        if (value >= 100)
            counter.text = "ПОЛНАЯ!";
        else
            counter.text = value.ToString();
    }

    public void Hide()
    {
        container.DOAnchorPosY(-344, ShowHideTime).SetEase(Ease.InOutQuad).Play();
    }

    public void Show()
    {
        container.DOAnchorPosY(332, ShowHideTime).SetEase(Ease.InOutQuad).Play();
    }

    public void Upper()
    {
        container.DOAnchorPosY(746, UpperTime).SetEase(Ease.InOutQuad).Play();
    }
}