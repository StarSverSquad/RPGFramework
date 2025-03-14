using RPGF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplorerMovement : MonoBehaviour
{
    public bool CanWalk = true;
    public bool CanRun = true;
    public bool CanRotate = true;

    public float Speed = 10f;
    public float AccelerationFactor = 1.5f;

    public float ResultSpeed => !IsRun ? Speed : Speed * AccelerationFactor;

    public bool IsMoving { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsAutoMoving { get; private set; } = false;

    public Vector2 Velocity = Vector2.zero;
    public Vector2 NormolizedVelocity = Vector2.zero;

    public MoveDirection MoveDirection;
    public ViewDirection ViewDirection = ViewDirection.Down;

    public event Action OnMoving;
    public event Action OnStopMoving;
    public event Action OnStartMoving;
    public event Action OnStartRun;
    public event Action OnStopRun;
    public event Action<ViewDirection> OnRotate;

    [SerializeField]
    private Rigidbody2D rb;

    private void FixedUpdate()
    {
        if (IsAutoMoving)
            return;

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

    public void SetMovementAccess(bool active)
    {
        CanRun = active;
        CanWalk = active;
        CanRotate = active;
    }

    public void TranslateBySpeed(Vector2 vec, float speed)
    {
        if (!IsAutoMoving)
            StartCoroutine(AutoMoveCoroutine(vec, speed));
    }
    public void TranslateByTime(Vector2 vec, float time)
    {
        if (!IsAutoMoving)
            StartCoroutine(AutoMoveCoroutine(vec, vec.sqrMagnitude / time));
    }

    public void RotateTo(ViewDirection direction)
    {
        ViewDirection = direction;

        OnRotate?.Invoke(direction);
    }

    

    private IEnumerator AutoMoveCoroutine(Vector2 vector, float speed)
    {
        IsAutoMoving = true;

        Vector2 vecDirection = vector.normalized;

        MoveDirection moveDir = DirectionConverter.GetMoveDiretionByVector(vecDirection);

        if (moveDir != MoveDirection.None)
        {
            var view = DirectionConverter.MoveDiretionToViewDiretion(moveDir);

            OnRotate?.Invoke(view);
            ViewDirection = view;
        }           

        float sqrMag = (float)Math.Sqrt(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2));

        float time = sqrMag / speed;

        OnStartMoving?.Invoke();

        while (time > 0)
        {
            transform.Translate(vecDirection * speed * Time.fixedDeltaTime);

            OnMoving?.Invoke();

            time -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        OnStopMoving?.Invoke();

        IsAutoMoving = false;
    }
}
