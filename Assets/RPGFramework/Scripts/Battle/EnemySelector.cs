using DG.Tweening;
using System;
using UnityEngine;

public class EnemySelector : RPGFrameworkBehaviour, IDisposable
{
    [SerializeField]
    private RectTransform _selector;
    [Space]
    public float RotationTime = 4f;
    public float SizeTime = 0.6f;
    public float SizeDelta = 100f;

    private Tween rotateTween;
    private Tween sizeTween;

    private Vector2 defaultSizeDelta;

    public override void Initialize()
    {
        defaultSizeDelta = _selector.sizeDelta;

        _selector.gameObject.SetActive(false);
    }

    public void Select(EnemyModel model)
    {
        Dispose();

        _selector.gameObject.SetActive(true);

        _selector.transform.position = model.AttackGlobalPoint;

        rotateTween = _selector
            .DORotate(new Vector3(0, 0, 360), RotationTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        sizeTween = _selector
            .DOSizeDelta(new Vector3(SizeDelta, SizeDelta, SizeDelta), SizeTime)
            .SetRelative()
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Dispose()
    {
        rotateTween?.Kill();
        sizeTween?.Kill();

        _selector.transform.rotation = Quaternion.identity;
        _selector.sizeDelta = defaultSizeDelta;

        _selector.gameObject.SetActive(false);
    }
}
