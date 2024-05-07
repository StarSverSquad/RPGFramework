using System.Collections;
using UnityEngine;

public class BossPattern2 : RPGAttackPattern
{
    [SerializeField]
    private GameObject sword;

    protected override IEnumerator PatternCoroutine()
    {
        float bulletSpeed = 3f;

        BattleManager.Instance.battleField.Resize(new Vector2(0.5f, 0.5f));
        BattleManager.Instance.battleField.Resize(new Vector2(4, 4), 4);

        while (true)
        {
            float angle = Random.Range(0f, 360f);

            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * new Vector2(0f, 1f);

            GameObject objbullet = CreateObject(sword, (new Vector2(0, -0.3f) + dir) * 5f);

            DirectionalBullet bullet = objbullet.GetComponent<DirectionalBullet>();

            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
            bullet.Direction = new Vector2(0f, 1f) * bulletSpeed;

            yield return new WaitForSeconds(0.25f);
        }
    }
}
