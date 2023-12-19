using System.Collections;
using UnityEngine;

public class KeparicPattern0 : RPGAttackPattern
{
    [SerializeField]
    private PatternBullet KeparicBullet;

    public float TimeOffset = 1f;

    private void Start()
    {
        Invoke();
    }

    protected override IEnumerator PatternCoroutine()
    {
        BattleManager.instance.battleField.Resize(new Vector2(0.5f, 0.5f));
        BattleManager.instance.battleField.Resize(new Vector2(1.5f, 3), 4f);

        bool right = true;

        yield return new WaitForSeconds(1f);

        while (true)
        {
            KepkaBullet blt;

            if (right)
                blt = CreateObject(KeparicBullet.gameObject, new Vector2(1f, 2f)).GetComponent<KepkaBullet>();
            else
                blt = CreateObject(KeparicBullet.gameObject, new Vector2(-1f, 2f)).GetComponent<KepkaBullet>();

            blt.Initialize(right);

            right = !right;
            

            yield return new WaitForSeconds(TimeOffset);
        }
    }
}