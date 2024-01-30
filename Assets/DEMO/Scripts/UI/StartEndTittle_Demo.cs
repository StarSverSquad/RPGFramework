using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndTittle_Demo : UISectionBase
{
    public float WaitTime = 2f;

    public UISectionBase NextSecction;

    public override void Initialize()
    {
        if (IsInitialized) return;

        isInitialized = true;

        OnEnter.Invoke();
    }

    public void StartEnd(bool isEnd) => StartCoroutine(StartEndCoroutine(isEnd));

    private IEnumerator StartEndCoroutine(bool isEnd)
    {
        if (isEnd)
        {
            yield return new WaitForSeconds(WaitTime);

            Deinitialize();
        }
        else
        {
            yield return new WaitForSeconds(WaitTime);

            InitializeChild(NextSecction);
        }
    }
}
