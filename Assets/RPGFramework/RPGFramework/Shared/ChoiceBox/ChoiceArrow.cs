using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceArrow : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void Shake() => animator.SetTrigger("SHAKE");
}
