using DG.Tweening;
using RPGF;
using System;
using UnityEngine;

public class PlayerExplorerMovement : MonoBehaviour
{
    public bool CanWalk = true;
    public bool CanRun = true;
    public bool CanRotate = true;

    public float Speed = 10f;
    public float AccelerationFactor = 1.5f;

    public Vector2 Velocity = Vector2.zero;
    public Vector2 NormolizedVelocity = Vector2.zero;

    public MoveDirection MoveDirection = MoveDirection.None;
    public ViewDirection ViewDirection = ViewDirection.Down;

    [SerializeField]
    private Rigidbody2D rb;

    #region PROPS

    public float ResultSpeed => !IsRun ? Speed : Speed * AccelerationFactor;
    public bool IsMoving { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsAutoMoving => autoMoveTween != null;

    #endregion

    #region EVENTS
    public event Action OnMoving;
    public event Action OnStopMoving;
    public event Action OnStartMoving;
    public event Action OnStartRun;
    public event Action OnStopRun;
    public event Action<ViewDirection> OnRotate;
    #endregion

    private Tween autoMoveTween = null;

    private void FixedUpdate()
    {
        if (IsAutoMoving)
        {
            OnMoving?.Invoke();
        }
        else
        {
            Move();
        }
    }

    public void SetMovementAccess(bool active)
    {
        CanRun = active;
        CanWalk = active;
        CanRotate = active;
    }

    public void TranslateBySpeed(Vector2 vec, float speed)
    {
        TranslateByTime(vec, vec.magnitude / speed);
    }
    public void TranslateByTime(Vector2 vec, float time)
    {
        DisposeAutoMoveTween();

        ViewDirection moveDir = DirectionConverter.GetViewDirectionByVector(vec.normalized);

        RotateTo(moveDir);

        autoMoveTween = transform.DOMove(vec, time).SetRelative().SetEase(Ease.Linear);

        autoMoveTween.onPlay += () =>
        {
            OnStartMoving?.Invoke();
        };

        autoMoveTween.onComplete += () =>
        {
            OnStopMoving?.Invoke();
            DisposeAutoMoveTween();
        };

        autoMoveTween.Play();
    }

    public void RotateTo(ViewDirection direction)
    {
        ViewDirection = direction;

        OnRotate?.Invoke(direction);
    }

    private void Move()
    {
        Velocity = Vector2.zero;

        MoveDirection = MoveDirection.None;
        NormolizedVelocity = Vector2.zero;

        ViewDirection? newViewDirection = null;

        if (CanWalk && !ExplorerManager.Instance.EventHandler.EventRuning)
        {

            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveRight))
            {
                MoveDirection = MoveDirection.Right;
                newViewDirection = ViewDirection.Right;
                Velocity += new Vector2(1, 0);
            }

            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveLeft))
            {
                MoveDirection = MoveDirection.Left;
                newViewDirection = ViewDirection.Left;
                Velocity += new Vector2(-1, 0);
            }

            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveUp))
            {
                MoveDirection = MoveDirection.Up;
                newViewDirection = ViewDirection.Up;
                Velocity += new Vector2(0, 1);
            }

            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveDown))
            {
                MoveDirection = MoveDirection.Down;
                newViewDirection = ViewDirection.Down;
                Velocity += new Vector2(0, -1);
            }
        }

        if (Velocity.magnitude > 0)
        {
            if (!IsMoving)
                OnStartMoving?.Invoke();

            IsMoving = true;

            OnMoving?.Invoke();
        }
        else
        {
            IsMoving = false;

            OnStopMoving?.Invoke();
        }


        if (CanRotate && newViewDirection.HasValue)
        {
            if (ViewDirection != newViewDirection)
            {
                ViewDirection = newViewDirection.Value;
                OnRotate?.Invoke(newViewDirection.Value);
            }
        }

        NormolizedVelocity = Velocity.normalized;

        bool nRun = Input.GetKey(GameManager.Instance.BaseOptions.Run) && CanRun;

        if (nRun && !IsRun)
            OnStartRun?.Invoke();
        else if (!nRun && IsRun)
            OnStopRun?.Invoke();

        IsRun = nRun;

        Velocity = ResultSpeed * Velocity.normalized;

        rb.linearVelocity = Velocity;
    }

    private void DisposeAutoMoveTween()
    {
        if (autoMoveTween != null)
        {
            autoMoveTween.Kill();
            autoMoveTween = null;
        }
    }
}
