using System.Collections;
using UnityEngine;

public class TestPattern : RPGAttackPattern
{
    [SerializeField]
    private PatternBullet SomeBullet;

    public int BulletsCount = 15;
    public float TimeOffset = 1f;

    private Coroutine move = null;

    protected override IEnumerator PatternCoroutine()
    {
        float time = Mathf.Abs(PatternTime - TimeOffset) / (BulletsCount + 1);

        move = StartCoroutine(MoveField());

        while (true)
        {
            float y = Random.Range(-2.5f, 0.5f);

            CreateObjectRelativeCenter(SomeBullet.gameObject, new Vector2(-4, y));

            yield return new WaitForSeconds(time);
        }
    }

    protected override IEnumerator TinyPatternCoroutine()
    {
        float time = Mathf.Abs(PatternTime - TimeOffset) / (BulletsCount + 1);

        while (true)
        {
            float y = Random.Range(-2.5f, 0.5f);

            CreateObjectRelativeCenter(SomeBullet.gameObject, new Vector2(-4, y));

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator MoveField()
    {
        bool moveKey = false;

        while (true)
        {
            if (!BattleManager.Instance.battleField.IsRotating)
            {
                BattleManager.Instance.battleField.Rotate(0);
                BattleManager.Instance.battleField.Rotate(360, 16);
            }

            if (!BattleManager.Instance.battleField.IsTransforming)
            {
                if (moveKey)
                    BattleManager.Instance.battleField.Transform(new Vector2(-2, 0), 1);
                else
                    BattleManager.Instance.battleField.Transform(new Vector2(2, 0), 1);

                moveKey = !moveKey;
            }

            yield return null;
        }
    }
}