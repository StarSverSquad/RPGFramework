using RPGF.Battle.Pattern;
using System.Collections;
using UnityEngine;

public class TestPattern : BattleAttackPatternBase
{
    [SerializeField]
    private PatternBulletBase SomeBullet;

    public int BulletsCount = 15;
    public float TimeOffset = 1f;

    protected override IEnumerator PatternCoroutine()
    {
        float time = Mathf.Abs(PatternTime - TimeOffset) / (BulletsCount + 1);

        StartCoroutine(MoveField());

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
            if (!Battle.BattleField.IsRotating)
            {
                Battle.BattleField.Rotate(0);
                Battle.BattleField.Rotate(359, 4);
            }

            if (!Battle.BattleField.IsMoving)
            {
                if (moveKey)
                    Battle.BattleField.MoveRelative(new Vector2(-2, 0), 2);
                else
                    Battle.BattleField.MoveRelative(new Vector2(2, 0), 2);

                moveKey = !moveKey;
            }

            yield return null;
        }
    }
}