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

    public CommonDirection MoveDirection;
    public CommonDirection ViewDirection = CommonDirection.Down;

    public event Action OnMoving;
    public event Action OnStopMoving;
    public event Action OnStartMoving;
    public event Action<CommonDirection> OnRotate;

    [SerializeField]
    private Rigidbody2D rb;

    private void FixedUpdate()
    {
        if (IsAutoMoving)
            return;

        Velocity = Vector2.zero;

        MoveDirection = CommonDirection.None;
        NormolizedVelocity = Vector2.zero;

        CommonDirection newViewDirection = CommonDirection.None;

        if (CanWalk && !ExplorerManager.instance.eventHandler.EventRuning)
        {

            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveDirection = CommonDirection.Right;
                newViewDirection = CommonDirection.Right;
                Velocity += new Vector2(1, 0);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveDirection = CommonDirection.Left;
                newViewDirection = CommonDirection.Left;
                Velocity += new Vector2(-1, 0);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveDirection = CommonDirection.Up;
                newViewDirection = CommonDirection.Up;
                Velocity += new Vector2(0, 1);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveDirection = CommonDirection.Down;
                newViewDirection = CommonDirection.Down;
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
            

        if (CanRotate && newViewDirection != CommonDirection.None)
        {
            if (ViewDirection != newViewDirection)
            {
                ViewDirection = newViewDirection;
                OnRotate?.Invoke(newViewDirection);
            }
        }

        NormolizedVelocity = Velocity.normalized;

        IsRun = Input.GetKey(KeyCode.LeftShift) && CanRun;

        Velocity = ResultSpeed * Velocity.normalized;

        rb.velocity = Velocity;
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

    public void RotateTo(CommonDirection direction)
    {
        if (direction == CommonDirection.None)
            return;

        ViewDirection = direction;

        OnRotate?.Invoke(direction);
    }

    private IEnumerator AutoMoveCoroutine(Vector2 vec, float speed)
    {
        IsAutoMoving = true;

        Vector2 vecDirection = vec.normalized;

        CommonDirection vieDir = CommonDirection.None;

        if (vecDirection.y > 0.7)
        {
            vieDir = CommonDirection.Up;
        }
        else if (vecDirection.y < -0.7)
        {
            vieDir = CommonDirection.Down;
        }
        else if (vecDirection.x > 0)
        {
            vieDir = CommonDirection.Right;
        }
        else if (vecDirection.x < 0)
        {
            vieDir = CommonDirection.Left;
        }

        if (vieDir != CommonDirection.None)
        {
            OnRotate?.Invoke(vieDir);

            ViewDirection = vieDir;
        }           

        float sqrMag = (float)Math.Sqrt(Math.Pow(vec.x, 2) + Math.Pow(vec.y, 2));

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

public enum CommonDirection
{
    None, Up, Down, Right, Left
}
