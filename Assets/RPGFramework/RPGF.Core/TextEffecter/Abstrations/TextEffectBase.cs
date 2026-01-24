using RPGF.Core.Services;
using RPGF.Domain.DI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPGF.Core.TextEffecter.Abstractions
{
    public abstract class TextEffectBase : InjectionTarget
    {
        public TransformTextMeshService TextTransformer { get; private set; }

        public int StartLetter { get; protected set; }
        public int EndLetter { get; protected set; }

        public bool IsWorking => effectCoroutine != null;

        protected TextMeshProUGUI TextMesh;

        protected Coroutine effectCoroutine = null;

        private MonoBehaviour listener;

        public TextEffectBase(TextMeshProUGUI textMesh)
        {
            TextTransformer = new TransformTextMeshService(textMesh);
        }

        public void Invoke(MonoBehaviour listener, int startLetter = 0, int endLetter = -1)
        {
            this.listener = listener;

            if (IsWorking)
            {
                Debug.LogWarning("Text effect already started. (will be restarted)");
                Stop();
            }

            StartLetter = startLetter;
            EndLetter = endLetter > -1 ? endLetter : TextMesh.text.Length;

            effectCoroutine = listener.StartCoroutine(EffectCoroutine());
        }
        public void Stop()
        {
            if (!IsWorking)
                return;

            if (listener != null)
            {
                listener.StopCoroutine(effectCoroutine);
                effectCoroutine = null;
                listener = null;
            }

            OnEnd();
        }

        #region VIRTUALS

        protected virtual void OnStart() { }
        protected virtual void OnEnd() { }

        #endregion

        protected abstract IEnumerator Pipeline();

        private IEnumerator EffectCoroutine()
        {
            OnStart();

            yield return Pipeline();

            effectCoroutine = null;

            OnEnd();
        }
    }
}