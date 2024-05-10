using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIShake : MonoBehaviour
{
    [SerializeField]
    private RectTransform ShakeRect;

    private Coroutine shakeCoroutine = null;
    public bool IsShaking => shakeCoroutine != null;

    [SerializeField]
    private float baseOffsetX = 1f;
    [SerializeField]
    private float baseOffsetY = 1f;

    public void Shake(float force)
    {
        if (IsShaking)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine(force));
    }

    private IEnumerator ShakeCoroutine(float force)
    {
        float offsetX = baseOffsetX * force;
        float offsetY = baseOffsetY * force;

        while (true)
        {
            ShakeRect.anchoredPosition = new Vector2(offsetX, offsetY);

            yield return new WaitForSeconds(0.02f);

            ShakeRect.anchoredPosition = new Vector2(offsetX, offsetY);

            yield return new WaitForSeconds(0.02f);

            offsetX -= offsetX / force;
            offsetY -= offsetY / force;

            if (offsetX < 0.005f && offsetY < 0.005f)
                break;
        }

        ShakeRect.anchoredPosition = new Vector2(0, 0);

        shakeCoroutine = null;

        yield break;
    }
}
