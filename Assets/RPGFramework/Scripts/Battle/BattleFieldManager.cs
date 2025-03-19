using DG.Tweening;
using System;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour, IActive, IDisposable
{
    [Tooltip("Up, Down, Left, Right")]
    [Header("Ссылки:")]
    [SerializeField]
    private BoxCollider2D[] boxColliders = new BoxCollider2D[4];
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Transform mask;
    [SerializeField]
    private SpriteRenderer background;

    [Header("Настройки:")]
    [SerializeField]
    private float Margin = 0.1f;
    [SerializeField]
    private Vector2 DefaultCenterOffset = Vector2.down;

    private Tween resizeTween;
    private Tween rotateTween;
    private Tween moveTween;

    public bool IsResizing => resizeTween != null;
    public bool IsRotating => rotateTween != null;
    public bool IsMoving => moveTween != null;

    public Vector2 StartPosition => (Vector2)Camera.main.transform.position + DefaultCenterOffset;

    public void SetActive(bool active)
    {
        container.SetActive(active);

        if (active)
        {
            Resize(new Vector2(3, 3));
            Rotate(0);
            MoveTo(StartPosition);
        }   
    }

    #region API

    public void Resize(Vector2 size, float time = 0, Ease ease = Ease.Linear)
    {
        DisposeResizeTween();

        if (time <= 0)
        {
            background.size = size;

            UpdateColliders();
        }
        else
        {
            resizeTween = DOTween
                .To(() => background.size, x => background.size = x, size, 1f)
                .SetEase(ease)
                .Play();

            resizeTween.onUpdate = () =>
            {
                UpdateColliders();
            };

            resizeTween.onComplete = () =>
            {
                DisposeResizeTween();
            };
        }
    }

    public void Rotate(float angle, float time = 0, Ease ease = Ease.Linear)
    {
        DisposeRotateTween();

        if (time <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            rotateTween = transform
                .DORotate(new Vector3(0, 0, angle), time, RotateMode.FastBeyond360)
                .SetEase(ease)
                .Play();

            rotateTween.onComplete = () =>
            {
                DisposeRotateTween();
            };
        }
    }

    public void MoveTo(Vector2 position, float time = 0, Ease ease = Ease.Linear)
    {
        DisposeMoveTween();

        if (time <= 0)
        {
            transform.position = position;
        }
        else
        {
            moveTween = transform
                .DOMove(position, time)
                .SetEase(ease)
                .Play();

            moveTween.onComplete = () =>
            {
                DisposeMoveTween();
            };
        }
    }
    public void MoveRelative(Vector2 offset, float time = 0, Ease ease = Ease.Linear)
    {
        MoveTo((Vector2)transform.position + offset, time, ease);
    }

    #endregion

    private void DisposeResizeTween()
    {
        if (resizeTween != null)
        {
            resizeTween.Kill();
            resizeTween = null;
        }
    }
    private void DisposeRotateTween()
    {
        if (rotateTween != null)
        {
            rotateTween.Kill();
            rotateTween = null;
        }
    }
    private void DisposeMoveTween()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
            moveTween = null;
        }
    }

    private void UpdateColliders()
    {
        boxColliders[0].offset = new Vector2(0, (background.size.y / 2f) - (Margin / 2f));
        boxColliders[0].size = new Vector2(background.size.x, Margin);

        boxColliders[1].offset = new Vector2(0, -(background.size.y / 2f) + (Margin / 2f));
        boxColliders[1].size = new Vector2(background.size.x, Margin);

        boxColliders[2].offset = new Vector2(-(background.size.x / 2f) + (Margin / 2f), 0);
        boxColliders[2].size = new Vector2(Margin, background.size.y);

        boxColliders[3].offset = new Vector2((background.size.x / 2f) - (Margin / 2f), 0);
        boxColliders[3].size = new Vector2(Margin, background.size.y);

        mask.localScale = new Vector2(background.size.x - Margin - Margin,
                                        background.size.y - Margin - Margin);
    }

    public void Dispose()
    {
        DisposeMoveTween();
        DisposeResizeTween();
        DisposeRotateTween();
    }

    private void OnDisable()
    {
        Dispose();
    }
}
