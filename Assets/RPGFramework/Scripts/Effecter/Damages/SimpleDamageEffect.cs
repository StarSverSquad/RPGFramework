using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDamageEffect : DamageEffect
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

    protected override IEnumerator DamageCoroutine()
    {
        Color from = Color.red;

        Color dif = Color.white - from;

        Vector3 fromvec = new Vector3(1, 0, 0);
        Vector3 tovec = new Vector3(1, 1, 1);
        Vector3 difvec = new Vector3(dif.r, dif.g, dif.b);

        float time = difvec.sqrMagnitude / Speed;

        foreach (var item in images)
            item.color = from;

        while (time > 0)
        {
            yield return new WaitForFixedUpdate();

            fromvec = Vector3.MoveTowards(fromvec, tovec, Speed * Time.fixedDeltaTime);

            foreach (var item in images)
                item.color = new Color(fromvec.x, fromvec.y, fromvec.z);

            time -= Time.fixedDeltaTime;
        }

        EndCoroutinePart();
    }
}