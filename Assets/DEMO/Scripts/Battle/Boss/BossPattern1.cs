using System.Collections;
using UnityEngine;

public class BossPattern1 : RPGAttackPattern
{
    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private AnimationCurve curve;

    protected override IEnumerator PatternCoroutine()
    {
        float bulletSpeed = 6f;

        BattleManager.Instance.battleField.Resize(new Vector2(0.3f, 0.3f));
        BattleManager.Instance.battleField.Resize(new Vector2(3, 4), 5);

        bool up = true;

        while (true)
        {
            GameObject objbullet = CreateObject(sword, up ? new Vector2(4.3f, 7.5f) : new Vector2(-4.3f, -7.7f));

            DirectionalBullet bullet = objbullet.GetComponent<DirectionalBullet>();

            bullet.transform.rotation = Quaternion.Euler(0, 0, up ? 90 : -90);
            bullet.Direction = new Vector2(-1f, bullet.Direction.y) * bulletSpeed;

            bullet.StartCoroutine(BulletAnim(bullet));

            up = !up;

            yield return new WaitForSeconds(0.28f);
        }
    }

    IEnumerator BulletAnim(DirectionalBullet bullet)
    {
        float time = 0;

        float speed = 8.3f;

        while (time < 1f)
        {
            bullet.Direction = new Vector2(bullet.Direction.x, curve.Evaluate(time) * speed);

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;
        }

        yield return new WaitForSeconds(1f);

        time = 0;

        while (time < 1f)
        {
            bullet.Direction = new Vector2(bullet.Direction.x, -curve.Evaluate(time) * speed);

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;
        }

        yield return new WaitForSeconds(1f);

        Destroy(bullet.gameObject);
    }
}
