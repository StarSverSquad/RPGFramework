using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPGF.Misc
{
    public class FallingText : RPGFrameworkBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMesh;

        [SerializeField]
        private AnimationCurve curve;

        public bool DeleteOnEnd = true;

        public float DeletionDelay = 1f;
        public float Delay = 0.1f;
        public float Speed = 1f;
        public float Distance = 20f;

        private Sequence animationSequence;
        private Tween deletionTween;
        private TransformTextMeshService transformText;
        private float[] characterTimes;
        private bool[] characterActive;
        private Color32 startColor;
        private Color32 endColor;

        public bool IsAnimate => animationSequence != null && animationSequence.IsActive();

        public void Invoke(string text, Color32 startColor, Color32 endColor)
        {
            if (IsAnimate)
                return;

            textMesh.text = text;

            this.startColor = startColor;
            this.startColor.a = 255;

            this.endColor = endColor;
            this.endColor.a = 255;

            PlayAnimation();
        }

        public void Invoke(string text)
        {
            Invoke(text, Color.white, Color.white);
        }

        private void PlayAnimation()
        {
            KillTweens();

            transformText = new TransformTextMeshService(textMesh);
            transformText.ResetMesh();

            int count = transformText.CharactersCount;
            characterTimes = new float[count];
            characterActive = new bool[count];

            if (count > 0)
                characterActive[0] = true;

            ApplyMesh();

            float duration = Speed > 0f ? 1f / Speed : 0f;
            animationSequence = DOTween.Sequence();

            for (int i = 0; i < count; i++)
            {
                int index = i;
                float insertTime = Delay * i;

                if (index > 0)
                    animationSequence.InsertCallback(insertTime, () => characterActive[index] = true);

                animationSequence.Insert(
                    insertTime,
                    DOTween.To(
                            () => characterTimes[index],
                            value => characterTimes[index] = value,
                            1f,
                            duration)
                        .SetEase(Ease.Linear));
            }

            animationSequence.OnUpdate(ApplyMesh);
            animationSequence.OnComplete(OnAnimationComplete);
            animationSequence.Play();
        }

        private void OnAnimationComplete()
        {
            animationSequence = null;
            ApplyMesh();

            if (!DeleteOnEnd)
                return;

            deletionTween = DOVirtual.DelayedCall(DeletionDelay, () => Destroy(gameObject));
        }

        private void ApplyMesh()
        {
            if (transformText == null || characterTimes == null)
                return;

            transformText.ResetMesh();

            for (int i = 0; i < characterTimes.Length; i++)
            {
                if (!characterActive[i])
                {
                    Color32 hidden = Color.white;
                    hidden.a = 0;

                    transformText.SetCharacterColor(i, hidden);
                    transformText.SetCharacterPosition(i, Vector2.zero);
                    continue;
                }

                float time = characterTimes[i];
                Color32 current = Color.Lerp(startColor, endColor, time);

                transformText.SetCharacterColor(i, current);
                transformText.SetCharacterPosition(
                    i,
                    new Vector2(0, curve.Evaluate(time) * Distance));
            }

            transformText.UpdateMesh();
        }

        private void KillTweens()
        {
            animationSequence?.Kill();
            animationSequence = null;

            deletionTween?.Kill();
            deletionTween = null;
        }

        private void OnDestroy()
        {
            KillTweens();
        }
    }
}