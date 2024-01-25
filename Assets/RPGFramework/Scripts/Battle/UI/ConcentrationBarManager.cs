using System.Collections;
using TMPro;
using UnityEngine;

public class ConcentrationBarManager : MonoBehaviour
{
    [SerializeField]
    private LineBar bar;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI txt;

    public float AnimationTime = 0.2f;

    private Coroutine anim;
    public bool IsAnimating => anim != null;

    private void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        if (IsAnimating)
            StopCoroutine(anim);

        float conAspect = (float)BattleManager.Data.Concentration / (float)BattleManager.Data.MaxConcentration;

        animator.SetTrigger("BLINK");

        anim = StartCoroutine(AnimationPack.MoveToByTimeLerp(bar.Value, conAspect, AnimationTime, value =>
        {
            bar.SetValue(value);
        }, () => anim = null));

        txt.text = "Концентрация\n" + (conAspect < 1f ? $"{Mathf.RoundToInt(conAspect * 100)}%" : "<color=#06C100>MAX</color>");
    }

    private IEnumerator AnimCoroutine()
    {
        float goal = BattleManager.Data.Concentration / BattleManager.Data.MaxConcentration;
        float dif = goal - bar.Value;

        float speed = Mathf.Abs(dif / AnimationTime);

        float time = AnimationTime;


        while (time > 0)
        {
            yield return new WaitForFixedUpdate();

            bar.SetValue(Mathf.MoveTowards(bar.Value, goal, speed * Time.fixedDeltaTime));

            time -= Time.fixedDeltaTime;
        }

        anim = null;
    }
}