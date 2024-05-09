using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UchKickBoss : CustomActionBase
{
    [SerializeField]
    private Transform UchTransform;

    [SerializeField]
    private AudioClip KickSound;

    protected override IEnumerator ActionCoroutine()
    {
        GameManager.Instance.GameAudio.PlaySE(KickSound);

        UchTransform.DOMove((Vector2)UchTransform.position + new Vector2(-6, 0), 1).Play();
        UchTransform.DORotate(new Vector3(0, 0, 45), 1).Play();

        yield break;
    }
}
