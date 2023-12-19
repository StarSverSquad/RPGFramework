using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackEffect : MonoBehaviour
{
    [Serializable]
    public struct KeyFrame
    {
        public Sprite sprite;
        public AudioClip audio;
        public float frameTime;
    }

    [Tooltip("Ёффект будет происходить по центру экрана")]
    [SerializeField]
    private bool locateInCenter = false;
    public bool LocaleInCenter => locateInCenter;

    [SerializeField]
    private Image spriteRenderer;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<KeyFrame> keyframes = new List<KeyFrame>();

    [SerializeField]
    private bool isAnimating = false;
    public bool IsAnimating => isAnimating;

    public void Invoke()
    {
        StartCoroutine(AnimationCoroutine());
    }

    private IEnumerator AnimationCoroutine()
    {
        isAnimating = true;

        foreach (var frame in keyframes)
        {
            if (frame.sprite != null)
                spriteRenderer.sprite = frame.sprite;

            if (frame.audio != null)
            {
                audioSource.clip = frame.audio;
                audioSource.Play();
            }

            yield return new WaitForSeconds(frame.frameTime);
        }

        isAnimating = false;
    }
}
