using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleChoiceButton : CommonChoiceUIElement
{
    [SerializeField]
    private RectTransform[] FocusEffectElements = new RectTransform[2];

    private Sequence aS;
    private Sequence bS;

    private Vector2[] startSizesOfFocusEffect = new Vector2[2];

    private void Start()
    {
        startSizesOfFocusEffect[0] = FocusEffectElements[0].sizeDelta;
        startSizesOfFocusEffect[1] = FocusEffectElements[1].sizeDelta;

        OnFocus.AddListener(OnButtonFocus);
        OnLostFocus.AddListener(OnButtonUnFocus);
        OnLocked.AddListener(OnButtonLocked);
        OnUnlocked.AddListener(OnButtonUnLocked);
        OnSelected.AddListener(OnButtonSelected);
        OnFailSelect.AddListener(OnButtonFailSelect);

        OnButtonUnFocus();
    }

    private void OnButtonFailSelect()
    {
        
    }

    private void OnButtonSelected()
    {

    }

    private void OnButtonUnLocked()
    {
        FocusEffectElements[0].GetComponent<Image>().color = new Color(1, 0.82f, 0, 0.5f);
        FocusEffectElements[1].GetComponent<Image>().color = new Color(1, 0.82f, 0, 0.5f);
    }

    private void OnButtonLocked()
    {
        FocusEffectElements[0].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        FocusEffectElements[1].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    }

    private void OnButtonUnFocus()
    {
        aS.Kill(true);
        bS.Kill(true);

        FocusEffectElements[0].sizeDelta = startSizesOfFocusEffect[0];
        FocusEffectElements[1].sizeDelta = startSizesOfFocusEffect[1];

        FocusEffectElements[0].GetComponent<Image>().enabled = false;
        FocusEffectElements[1].GetComponent<Image>().enabled = false;
    }

    private void OnButtonFocus()
    {
        FocusEffectElements[0].GetComponent<Image>().enabled = true;
        FocusEffectElements[1].GetComponent<Image>().enabled = true;

        aS = DOTween.Sequence();
        bS = DOTween.Sequence();

        aS.Append(FocusEffectElements[0].DOSizeDelta(new Vector2(0, 20), 0.75f)
            .SetEase(Ease.InOutSine).SetRelative());

        aS.Append(FocusEffectElements[0].DOSizeDelta(new Vector2(0, -20), 0.75f)
            .SetEase(Ease.InOutSine).SetRelative());

        aS.SetLoops(-1).Play();

        bS.Append(FocusEffectElements[1].DOSizeDelta(new Vector2(0, 25), 0.75f)
            .SetEase(Ease.InOutSine).SetRelative());

        bS.Append(FocusEffectElements[1].DOSizeDelta(new Vector2(0, -25), 0.75f)
            .SetEase(Ease.InOutSine).SetRelative());

        bS.SetLoops(-1).Play();
    }

    private void OnDestroy()
    {
        aS.Kill(true);
        bS.Kill(true);
    }
}