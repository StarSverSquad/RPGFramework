using RPGF.Battle.Pattern;
using System.Collections;
using UnityEngine;

[AddComponentMenu("RPG/Battle/EnemyBehaviour/KeparicPattern0")]
public class KeparicPattern0 : BattleEnemyBehaviourBase
{
    [SerializeField]
    private PatternBulletBase KeparicBullet;

    public float TimeOffset = 1f;

    protected override IEnumerator BehaviourCoroutine()
    {
        if (IsSingle)
            yield return MainPattern();
        else
            yield return TinyPatternCoroutine();
    }

    private IEnumerator MainPattern()
    {
        Battle.BattleField.Resize(new Vector2(1.5f, 3));

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

    private IEnumerator TinyPatternCoroutine()
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