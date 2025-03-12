using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UndertaleBattleTransmition : VisualBattleTransmitionEffectBase
{
    [SerializeField]
    private Image heart;
    [SerializeField]
    private Image back;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip tick;
    [SerializeField]
    private AudioClip fall;

    public override IEnumerator PartOne()
    {
        heart.transform.position = ExplorerManager.GetPlayerPosition3D() + new Vector3(0, 0.35f, 0);

        source.clip = tick;

        yield return new WaitForSeconds(0.1f);

        back.enabled = true;
        heart.enabled = true;

        source.Play();

        yield return new WaitForSeconds(0.1f);

        back.enabled = false;
        heart.enabled = false;

        source.Play();

        yield return new WaitForSeconds(0.1f);

        back.enabled = true;
        heart.enabled = true;

        source.Play();

        yield return new WaitForSeconds(0.1f);

        back.enabled = false;
        heart.enabled = false;

        source.Play();

        yield return new WaitForSeconds(0.1f);

        source.clip = fall;

        source.Play();

        back.enabled = true;
        heart.enabled = true;

        yield return StartCoroutine(AnimationPack.MoveToBySpeedLerp2D(heart.transform.position, transform.position, 2f, value =>
        {
            heart.transform.position = value;
        }));

        yield return new WaitForSeconds(1f);
    }

    public override IEnumerator PartTwo()
    {
        back.enabled = false;
        heart.enabled = false;

        yield break;
    }
}
