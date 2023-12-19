using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDeathEffect : DeathEffect
{
    private Image[] images;

    public float Speed = 4f;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
    }

    public override void Cleanup()
    {
        foreach (var image in images)
            image.color = Color.white;
    }

    protected override IEnumerator DeathCoroutine()
    {
        Color from = new Color(0.75f, 0.1f, 0.1f, 1);
        Color to = new Color(0.5f, 0.5f, 0.5f, 0);

        Color dif = to - from;

        Vector4 fromvec = new Vector4(0.75f, 0.1f, 0.1f, 1);
        Vector4 tovec = new Vector4(0.5f, 0.5f, 0.5f, 0);
        Vector4 difvec = new Vector4(dif.r, dif.g, dif.b, dif.a);

        float time = difvec.sqrMagnitude / Speed;

        foreach (var item in images)
            item.color = from;

        while (time > 0)
        {
            yield return new WaitForFixedUpdate();

            fromvec = Vector4.MoveTowards(fromvec, tovec, Speed * Time.fixedDeltaTime);

            foreach (var item in images)
                item.color = new Color(fromvec.x, fromvec.y, fromvec.z, fromvec.w);

            time -= Time.fixedDeltaTime;
        }

        EndCoroutinePart();
    }
}
