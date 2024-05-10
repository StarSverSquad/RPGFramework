using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicExplorerObject : MonoBehaviour
{
    #region NOT UNITY SERIALIZED

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public CommonDirection DirectionView { get; private set; }

    private Coroutine moveCoroutine;
    private Coroutine animationCoroutine;

    public bool MoveInPause { get; private set; } = false;

    public bool IsMove => moveCoroutine != null;
    public bool IsAnimation => animationCoroutine != null;

    #endregion

    [Header("Keyframes")]
    [SerializeField]
    private Sprite upIdle;
    [SerializeField]
    private Sprite downIdle;
    [SerializeField]
    private Sprite leftIdle;
    [SerializeField]
    private Sprite rightIdle;

    [SerializeField]
    private List<Sprite> upFrames = new List<Sprite>();
    [SerializeField]
    private List<Sprite> downFrames = new List<Sprite>();
    [SerializeField]
    private List<Sprite> leftFrames = new List<Sprite>();
    [SerializeField]
    private List<Sprite> rightFrames = new List<Sprite>();

    [Header("Options")]
    public CommonDirection DefaultDirection = CommonDirection.Down;

    [SerializeField]
    private ExplorerEvent reactedEvent;

    [SerializeField]
    private float animationSpeed;
    public float AnimationSpeed => animationSpeed;

    [SerializeField]
    private float speedFactor = 1f;
    public float SpeedFactor
    {
        get => speedFactor;
        set
        {
            speedFactor = value;

            if (IsAnimation)
                AnimateMove(DirectionView);
        }
    }

    private void Start()
    {
        if (reactedEvent == null) 
            TryGetComponent(out reactedEvent);

        TryGetComponent(out animator);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (reactedEvent != null)
        {
            reactedEvent.Event.OnStart += Event_OnStart;
            reactedEvent.Event.OnEnd += Event_OnEnd;
        }

        SetDefault();
    }

    private void OnDestroy()
    {
        if (reactedEvent != null)
        {
            reactedEvent.Event.OnEnd -= Event_OnEnd;
            reactedEvent.Event.OnStart -= Event_OnStart;
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    private void Event_OnEnd()
    {
        RotateTo(CommonDirection.None);
    }
    private void Event_OnStart()
    {
        RotateToPlayer();
    }

    public void SetDefault()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = null;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        animationCoroutine = null;

        if (DefaultDirection == CommonDirection.None)
            DefaultDirection = CommonDirection.Down;

        RotateTo(DefaultDirection);
    }

    public void TriggerAnimator(string trigger)
    {
        if (animator != null)
            animator.SetTrigger(trigger);
    }
    public void SetBoolAnimator(string var, bool val)
    {
        if (animator != null)
            animator.SetBool(var, val);
    }

    public void RotateToPlayer()
    {
        Vector2 dif = (ExplorerManager.GetPlayerPosition() - (Vector2)transform.position).normalized;

        if (dif.y >= 0.5f)
            RotateTo(CommonDirection.Up);
        else if (dif.y <= -0.5f)
            RotateTo(CommonDirection.Down);
        else if (dif.x >= 0.5f)
            RotateTo(CommonDirection.Right);
        else
            RotateTo(CommonDirection.Left);
    }

    public void RotateTo(CommonDirection direction)
    {
        DirectionView = direction;

        switch (direction)
        {
            case CommonDirection.Up:
                spriteRenderer.sprite = upIdle;
                break;
            case CommonDirection.Down:
                spriteRenderer.sprite = downIdle;
                break;
            case CommonDirection.Right:
                spriteRenderer.sprite = rightIdle;
                break;
            case CommonDirection.Left:
                spriteRenderer.sprite = leftIdle;
                break;
            default:
                RotateTo(DefaultDirection);
                break;
        }
    }

    public void AnimateMove(CommonDirection direction)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimationCoroutine(direction));
    }

    public void StopAnimation(CommonDirection rotDir = CommonDirection.None)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = null;

        RotateTo(rotDir == CommonDirection.None ? DirectionView : rotDir);
    }

    public void TranslateByTime(Vector2 moveVector, float time, bool isAnimate = true)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MovementCoroutine(moveVector, time, isAnimate));
    }
    public void TranslateBySpeed(Vector2 moveVector, float speed, bool isAnimate = true)
    {
        TranslateByTime(moveVector, moveVector.magnitude / speed, isAnimate);
    }

    public void MoveToByTime(Vector2 position, float time, bool isAnimate = true)
    {
        Vector2 moveVector = position - (Vector2)transform.position;

        TranslateByTime(moveVector, time, isAnimate);
    }
    public void MoveToBySpeed(Vector2 position, float speed, bool isAnimate = true)
    {
        Vector2 moveVector = position - (Vector2)transform.position;

        TranslateBySpeed(moveVector, speed, isAnimate);
    }

    public void PauseMove(bool stopAnimation = true)
    {
        if (IsMove)
        {
            MoveInPause = true;

            if (stopAnimation) 
                StopAnimation();
        }
    }
    public void UnpauseMove(bool startAnimation = true)
    {
        if (IsMove)
        {
            MoveInPause = false;

            if (startAnimation)
                AnimateMove(DirectionView);
        }
    }

    private IEnumerator AnimationCoroutine(CommonDirection direction)
    {
        List<Sprite> list;

        switch (direction)
        {
            case CommonDirection.Up:
                list = upFrames;
                break;
            case CommonDirection.Down:
                list = downFrames;
                break;
            case CommonDirection.Right:
                list = rightFrames;
                break;
            case CommonDirection.Left:
                list = leftFrames;
                break;
            default:
                yield break;
        }

        float oneFrameTime = 1 / (AnimationSpeed * speedFactor);

        int keyframe = 0;

        while (true)
        {
            spriteRenderer.sprite = list[keyframe];

            if (keyframe < list.Count - 1)
                keyframe++;
            else
                keyframe = 0;

            yield return new WaitForSeconds(oneFrameTime);
        }
    }

    private IEnumerator MovementCoroutine(Vector2 moveVector, float time, bool isAnimate = true)
    {
        float speed = moveVector.magnitude / time;

        float ct = time;

        MoveInPause = false;

        CommonDirection direction;

        if (moveVector.normalized.y >= 0.5f)
            direction = CommonDirection.Up;
        else if (moveVector.normalized.y <= -0.5f)
            direction = CommonDirection.Down;
        else if (moveVector.normalized.x >= 0.5f)
            direction = CommonDirection.Right;
        else
            direction = CommonDirection.Left;

        if (isAnimate)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            DirectionView = direction;

            AnimateMove(direction);
        }

        while (ct > 0)
        {
            yield return new WaitForFixedUpdate();

            transform.Translate(moveVector.normalized * speed * Time.fixedDeltaTime);

            ct -= Time.fixedDeltaTime;

            yield return new WaitWhile(() => MoveInPause);
        }

        StopAnimation();

        moveCoroutine = null;
    }
}
