using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterRenderer : MonoBehaviour
{
    [Header("Links")]
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private ExplorerEvent someEvent;

    [Header("Keyframes")]
    public Sprite IdleUp;
    public Sprite IdleDown;
    public Sprite IdleLeft;
    public Sprite IdleRight;

    public List<Sprite> MoveUpKeyframes = new List<Sprite>();
    public List<Sprite> MoveDownKeyframes = new List<Sprite>();
    public List<Sprite> MoveLeftKeyframes = new List<Sprite>();
    public List<Sprite> MoveRightKeyframes = new List<Sprite>();

    [Header("Options")]
    public float AnimationSpeed = 1;
    public float AcceleratedAnimationSpeed = 1.5f;

    public CommonDirection DefaultDirection;

    [SerializeField]
    private CommonDirection currentDirection;
    public CommonDirection CurrentDirection => currentDirection;

    private Coroutine animCoroutine = null;

    public bool IsWalking => animCoroutine != null;

    [SerializeField]
    private bool isAccelerated = false;
    public bool IsAccelerated { get => isAccelerated; set => isAccelerated = value; }

    private void Start()
    {
        someEvent ??= GetComponent<ExplorerEvent>();

        if (someEvent != null)
        {
            someEvent.Event.OnStart += Event_OnStart;
            someEvent.Event.OnEnd += Event_OnEnd;
        }

        SetDefault();
    }

    private void OnDestroy()
    {
        if (someEvent != null)
        {
            someEvent.Event.OnEnd -= Event_OnEnd;
            someEvent.Event.OnStart -= Event_OnStart;
        }
    }

    private void Event_OnEnd()
    {
        Rotate(CommonDirection.None);
    }

    private void Event_OnStart()
    {
        RotateToPlayer();
    }

    public void RotateToPlayer()
    {
        Vector2 dif = (ExplorerManager.GetPlayerPosition() - (Vector2)transform.position).normalized;

        Debug.Log("asd");

        if (dif.y >= 0.5f)
            Rotate(CommonDirection.Up);
        else if (dif.y <= -0.5f)
            Rotate(CommonDirection.Down);
        else if (dif.x >= 0.5f)
            Rotate(CommonDirection.Right);
        else
            Rotate(CommonDirection.Left);
    }

    public void StartWalk()
    {
        if (IsWalking)
            return;

        animCoroutine = StartCoroutine(AnimationCoroutine());
    }
    public void StopWalk()
    {
        if (!IsWalking)
            return;

        StopCoroutine(animCoroutine);
        animCoroutine = null;

        ChangeDirection();
    }

    public void RefreshWalk()
    {
        if (IsWalking)
        {
            StopCoroutine(animCoroutine);
            animCoroutine = null;

            animCoroutine = StartCoroutine(AnimationCoroutine());
        }
    }

    public void SetDefault()
    {
        if (IsWalking)
            StopWalk();

        Rotate(DefaultDirection);
    }

    public void Rotate(CommonDirection direction)
    {
        currentDirection = direction == CommonDirection.None ? DefaultDirection : direction;

        if (IsWalking)
        {
            RefreshWalk();
        }
        else
        {
            switch (currentDirection)
            {
                case CommonDirection.Up:
                    sr.sprite = IdleUp;
                    break;
                case CommonDirection.Down:
                    sr.sprite = IdleDown;
                    break;
                case CommonDirection.Right:
                    sr.sprite = IdleRight;
                    break;
                case CommonDirection.Left:
                    sr.sprite = IdleLeft;
                    break;
            }
        }
    }
    public void ChangeDirection()
    {
        Rotate(currentDirection);
    }

    private IEnumerator AnimationCoroutine()
    {
        List<Sprite> list;

        switch (CurrentDirection)
        {
            case CommonDirection.Up:
                list = MoveUpKeyframes;
                break;
            case CommonDirection.Down:
                list = MoveDownKeyframes;
                break;
            case CommonDirection.Right:
                list = MoveRightKeyframes;
                break;
            case CommonDirection.Left:
                list = MoveLeftKeyframes;
                break;
            default:
                yield break;
        }

        float oneFrameTime = list.Count / (isAccelerated ? AcceleratedAnimationSpeed : AnimationSpeed) / list.Count;

        int keyframe = 0;

        while (true)
        {
            sr.sprite = list[keyframe];

            if (keyframe < list.Count - 1)
                keyframe++;
            else
                keyframe = 0;

            yield return new WaitForSeconds(oneFrameTime);
        }
    }
}
