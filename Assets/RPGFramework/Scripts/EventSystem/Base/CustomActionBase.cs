using System;
using System.Collections;
using UnityEngine;

public abstract class CustomActionBase : MonoBehaviour
{
    public string ActionTag;

    public Coroutine Invoke() => StartCoroutine(ActionCoroutine());

    protected abstract IEnumerator ActionCoroutine();
}