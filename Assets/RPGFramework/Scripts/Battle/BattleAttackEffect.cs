using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttackEffect : MonoBehaviour
{
    [SerializeField]
    private Image spriteRenderer;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Animator animator;

    [Space]
    [SerializeField]
    private string animatorTriggerName = "START";
    [SerializeField]
    private string animatorIdleStateName = "IDLE";

    [Space]
    [Tooltip("������ ����� ����������� �� ������ ������")]
    [SerializeField]
    private bool locateInCenter = false;
    public bool LocaleInCenter => locateInCenter;

    private bool isAnimating = false;
    public bool IsAnimating => isAnimating;

    public void Invoke()
    {
        StartCoroutine(AnimationCoroutine());
    }

    private IEnumerator AnimationCoroutine()
    {
        isAnimating = true;

        animator.SetTrigger(animatorTriggerName);

        audioSource.Play();

        yield return new WaitForSeconds(0.01f);

        yield return new WaitWhile(() => !animator.GetCurrentAnimatorStateInfo(0).IsName(animatorIdleStateName));

        isAnimating = false;
    }
}
