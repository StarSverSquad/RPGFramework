using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BossPattern3 : RPGAttackPattern
{
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject sheald;

    [SerializeField]
    private AudioClip bell;
    [SerializeField]
    private AudioClip swordAproach;

    protected override IEnumerator PatternCoroutine()
    {
        BattleManager.Instance.battleField.Resize(new Vector2(0.5f, 0.5f));
        BattleManager.Instance.battleField.Resize(new Vector2(3, 3), 3f);

        yield return new WaitForSeconds(1f);

        GameObject shiled = CreateObjectRelativeBattleField(sheald, new Vector2(-6, 0));
        SpriteRenderer shieldRenderer = shiled.GetComponent<SpriteRenderer>();

        shiled.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        shieldRenderer.color = new Color(1, 1, 1, 0);

        BattleManager.Instance.battleAudio.PlaySound(bell);

        shiled.transform.DOMove(BattleFieldPosition, 1).SetEase(Ease.OutExpo).Play();
        shiled.transform.DORotate(Vector3.zero, 1).SetEase(Ease.OutExpo).Play();
        shieldRenderer.DOColor(new Color(1, 1, 1, 1), 0.5f).Play();

        yield return new WaitForSeconds(1.5f);

        for (int j = 0; j < 7; j++)
        {
            int direction = Random.Range(1, 5);

            BattleManager.Instance.battleAudio.PlaySound(swordAproach);

            for (int i = 0; i < 6; i++)
            {
                GameObject swordItem = null;

                switch (direction)
                {
                    case 1:
                        swordItem = CreateObjectRelativeBattleField(sword, new Vector2(1.3f - (0.5f * i), 4));
                        break;
                    case 2:
                        swordItem = CreateObjectRelativeBattleField(sword, new Vector2(4, 1.3f - (0.5f * i)));
                        break;
                    case 3:
                        swordItem = CreateObjectRelativeBattleField(sword, new Vector2(1.3f - (0.5f * i), -4));
                        break;
                    case 4:
                        swordItem = CreateObjectRelativeBattleField(sword, new Vector2(-4, 1.3f - (0.5f * i)));
                        break;
                }

                swordItem.GetComponent<PatternBullet>().StartCoroutine(SwordCoroutine(swordItem, i, direction));
            }

            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator SwordCoroutine(GameObject sword, int order, int direction)
    {
        SpriteRenderer renderer = sword.GetComponent<SpriteRenderer>();

        float startOffset = 0.5f;
        float halfPath = 2.6f;
        float fullPath = 12f;

        renderer.color = new Color(1, 1, 1, 0);

        switch (direction)
        {
            case 1:
                sword.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 2:
                sword.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3:
                sword.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 4:
                sword.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }

        renderer.DOColor(new Color(1, 1, 1, 1), 0.2f).Play();

        switch (direction)
        {
            case 1:
                sword.transform.DOMoveY(sword.transform.position.y + startOffset, 0.5f).Play();
                break;
            case 2:
                sword.transform.DOMoveX(sword.transform.position.x + startOffset, 0.5f).Play();
                break;
            case 3:
                sword.transform.DOMoveY(sword.transform.position.y - startOffset, 0.5f).Play();
                break;
            case 4:
                sword.transform.DOMoveX(sword.transform.position.x - startOffset, 0.5f).Play();
                break;
        }

        yield return new WaitForSeconds(0.5f);

        if (order == 2 || order == 3)
        {
            switch (direction)
            {
                case 1:
                    sword.transform.DOMoveY(sword.transform.position.y - halfPath, 0.15f).SetEase(Ease.Linear).Play();
                    break;
                case 2:
                    sword.transform.DOMoveX(sword.transform.position.x - halfPath, 0.15f).SetEase(Ease.Linear).Play();
                    break;
                case 3:
                    sword.transform.DOMoveY(sword.transform.position.y + halfPath, 0.15f).SetEase(Ease.Linear).Play();
                    break;
                case 4:
                    sword.transform.DOMoveX(sword.transform.position.x + halfPath, 0.15f).SetEase(Ease.Linear).Play();
                    break;
            }

            yield return new WaitForSeconds(0.15f);

            BattleManager.Instance.battleAudio.PlaySound(bell);

            renderer.DOColor(new Color(1, 1, 1, 0), 0.2f).Play();
            sword.transform.DORotate(sword.transform.rotation.eulerAngles - new Vector3(0, 0, -70), 0.2f).SetEase(Ease.InExpo).Play();

            switch (direction)
            {
                case 1:
                    sword.transform.DOMoveY(sword.transform.position.y + startOffset * 2, 0.2f).Play();
                    break;
                case 2:
                    sword.transform.DOMoveX(sword.transform.position.x + startOffset * 2, 0.2f).Play();
                    break;
                case 3:
                    sword.transform.DOMoveY(sword.transform.position.y - startOffset * 2, 0.2f).Play();
                    break;
                case 4:
                    sword.transform.DOMoveX(sword.transform.position.x - startOffset * 2, 0.2f).Play();
                    break;
            }

            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            switch (direction)
            {
                case 1:
                    sword.transform.DOMoveY(sword.transform.position.y - fullPath, 1.3f).Play();
                    break;
                case 2:
                    sword.transform.DOMoveX(sword.transform.position.x - fullPath, 1.3f).Play();
                    break;
                case 3:
                    sword.transform.DOMoveY(sword.transform.position.y + fullPath, 1.3f).Play();
                    break;
                case 4:
                    sword.transform.DOMoveX(sword.transform.position.x + fullPath, 1.3f).Play();
                    break;
            }

            yield return new WaitForSeconds(1.3f);
        }

        Destroy(sword);
    }
}