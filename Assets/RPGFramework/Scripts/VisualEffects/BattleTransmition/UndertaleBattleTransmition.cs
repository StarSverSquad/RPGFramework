﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

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

        float time = Vector2.Distance(transform.position, heart.transform.position) / 2f;

        heart.transform.DOMove(transform.position, time).Play();

        yield return new WaitForSeconds(time);
    }

    public override IEnumerator PartTwo()
    {
        back.enabled = false;
        heart.enabled = false;

        yield break;
    }
}
