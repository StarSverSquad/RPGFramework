using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIConcentration : MonoBehaviour
{
    [SerializeField]
    private RectTransform barBg;
    [SerializeField]
    private RectTransform barFront;

    [SerializeField]
    private TextMeshProUGUI counter;

    public void SetConcentration(int value)
    {
        float yPos = (value / 100) * barBg.anchoredPosition.y;

        barFront.DOAnchorPosY(yPos, 0.15f).SetLoops(0).SetEase(Ease.OutSine).Play();
        barFront.GetComponent<Image>().DOColor(new Color(.1f, 0.52f, 0), 0.15f)
            .From(new Color(.16f, 0.72f, 0.014f)).SetLoops(0).SetEase(Ease.OutSine).Play();

        if (value >= 100)
            counter.text = "ПОЛНАЯ!";
        else
            counter.text = value.ToString();
    }
}