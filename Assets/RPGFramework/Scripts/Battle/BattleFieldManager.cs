using System;
using System.Collections;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour, IActive
{
    [Tooltip("Up, Down, Left, Right")]
    [SerializeField]
    private BoxCollider2D[] boxColliders = new BoxCollider2D[4];

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private Transform mask;

    [SerializeField]
    private SpriteRenderer background;

    public Vector4 Margin;

    private Coroutine resizeCoroutine = null;

    public bool IsResizing => resizeCoroutine != null;

    public void SetActive(bool active)
    {
        container.SetActive(active);

        if (active)
        {
            Resize(new Vector2(3, 3));
            transform.position = (Vector2)Camera.main.transform.position;
        }
            
    }

    private void OnBackgroundResized()
    {
        boxColliders[0].offset = new Vector2(0, (background.size.y / 2f) - (Margin.y / 2f));
        boxColliders[0].size = new Vector2(background.size.x, Margin.y);

        boxColliders[1].offset = new Vector2(0, -(background.size.y / 2f) + (Margin.w / 2f));
        boxColliders[1].size = new Vector2(background.size.x, Margin.w);

        boxColliders[2].offset = new Vector2(-(background.size.x / 2f) + (Margin.x / 2f), 0);
        boxColliders[2].size = new Vector2(Margin.x, background.size.y);

        boxColliders[3].offset = new Vector2((background.size.x / 2f) - (Margin.z / 2f), 0);
        boxColliders[3].size = new Vector2(Margin.z, background.size.y);

        mask.localScale = new Vector2(background.size.x - Margin.x - Margin.z,
                                        background.size.y - Margin.y - Margin.w);
    }

    public void Resize(Vector2 size, float speed = 0)
    {
        if (IsResizing)
        {
            StopCoroutine(resizeCoroutine);
            resizeCoroutine = null;
        }

        if (speed <= 0)
        {
            background.size = size;

            OnBackgroundResized();
        }
        else
        {
            resizeCoroutine = StartCoroutine(Anims.MoveToBySpeed2D(background.size, size, speed, val =>
            {
                background.size = val;
                OnBackgroundResized();
            }, () => resizeCoroutine = null));
        }
    }

    //private IEnumerator ResizeCoroutine(Vector2 size, float speed)
    //{
    //    float resizeTime = Math.Abs((size - background.size).sqrMagnitude) / speed;

    //    while (resizeTime > 0)
    //    {
    //        yield return new WaitForFixedUpdate();

    //        background.size = Vector2.MoveTowards(background.size, size, speed * Time.fixedDeltaTime);

    //        resizeTime -= Time.fixedDeltaTime;

    //        OnBackgroundResized();
    //    }
    //}
}
