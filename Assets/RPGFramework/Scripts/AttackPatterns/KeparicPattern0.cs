using System.Collections;
using UnityEngine;

public class KeparicPattern0 : RPGAttackPattern
{
    [SerializeField]
    private PatternBullet KeparicBullet;

    public float TimeOffset = 1f;

    protected override IEnumerator PatternCoroutine()
    {
        BattleManager.Instance.BattleField.Resize(new Vector2(0.5f, 0.5f));
        BattleManager.Instance.BattleField.Resize(new Vector2(1.5f, 3), 4f);

        bool right = true;

        yield return new WaitForSeconds(1f);

        while (true)
        {
            KepkaBullet blt;

            if (right)
                blt = CreateObjectRelativeCenter(KeparicBullet.gameObject, new Vector2(1f, 1.5f)).GetComponent<KepkaBullet>();
            else
                blt = CreateObjectRelativeCenter(KeparicBullet.gameObject, new Vector2(-1f, 1.5f)).GetComponent<KepkaBullet>();

            blt.Initialize(right);

            right = !right;
            

            yield return new WaitForSeconds(TimeOffset);
        }
    }

    protected override IEnumerator TinyPatternCoroutine()
    {
        bool right = false;

        yield return new WaitForSeconds(1f);

        while (true)
        {
            KepkaBullet blt;

            if (right)
                blt = CreateObjectRelativeCenter(KeparicBullet.gameObject, new Vector2(2f, 1.5f)).GetComponent<KepkaBullet>();
            else
                blt = CreateObjectRelativeCenter(KeparicBullet.gameObject, new Vector2(-2f, 1.5f)).GetComponent<KepkaBullet>();

            blt.Initialize(right);

            right = !right;


            yield return new WaitForSeconds(TimeOffset);
        }
    }
}