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

    public float ShowHideTime = 0.75f;
    public float NearWindowTime = 0.25f;

    public Color BarNormal;
    public Color BarDark;

    public void SetConcentration(int value)
    {
        float ySize = (value / 100f) * barBg.sizeDelta.y;

        barFront.DOSizeDelta(new Vector2(barFront.sizeDelta.x, ySize), 0.15f).SetEase(Ease.OutCirc).Play();

        barFront.GetComponent<Image>().DOColor(BarNormal, 0.4f).From(BarDark).SetEase(Ease.OutCirc).Play();

        if (value >= 100)
            counter.text = "ПОЛНАЯ!";
        else
            counter.text = value.ToString();
    }

    public void Hide()
    {
        container.DOAnchorPosX(-400, ShowHideTime).SetEase(Ease.InOutQuad).Play();
    }

    public void Show()
    {
        container.DOAnchorPosX(150, ShowHideTime).SetEase(Ease.InOutQuad).Play();
    }

    public void NearWindow()
    {
        container.DOAnchorPosX(-160, NearWindowTime).SetEase(Ease.InOutQuad).Play();
    }
}