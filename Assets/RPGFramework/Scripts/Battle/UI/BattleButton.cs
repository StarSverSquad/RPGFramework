using System;
using System.Collections;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool isShow = false;
    public bool IsShow => isShow;

    private void OnEnable()
    {
        isShow = false;
    }

    public void Show()
    {
        animator.SetTrigger("SHOW");

        isShow = true;
    }

    public void Hide()
    {
        animator.SetTrigger("HIDE");

        isShow = false;
    }
}