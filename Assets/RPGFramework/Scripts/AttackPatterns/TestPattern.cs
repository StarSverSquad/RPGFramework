using System.Collections;
using UnityEngine;

public class TestPattern : RPGAttackPattern
{
    [SerializeField]
    private PatternBullet SomeBullet;

    public int BulletsCount = 15;
    public float TimeOffset = 1f;

    protected override IEnumerator PatternCoroutine()
    {
        float time = Mathf.Abs(PatternTime - TimeOffset) / (BulletsCount + 1);

        while (true)
        {
            float y = Random.Range(-1.5f, 1.5f);

            CreateObject(SomeBullet.gameObject, new Vector2(-2, y));

            yield return new WaitForSeconds(time);
        }
    }
}