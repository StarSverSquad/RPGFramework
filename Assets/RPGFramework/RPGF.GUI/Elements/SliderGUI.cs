using System.Collections;
using NaughtyAttributes;
using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI.Elements
{
    public class SliderGUI : GUIBlock
    {
        [Inject]
        private readonly BaseOptions _options = null!;

        [SerializeField]
        private RectTransform barBack;
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private RectTransform cursor;
        [SerializeField]
        private float step = 0.05f;

        private float entryValue;
        private Coroutine inputCoroutine;

        public float Value { get; private set; }

        [Foldout("Slider events")]
        public UnityEvent<float> OnSliderChanged;
        [Foldout("Slider events")]
        public UnityEvent<float> OnSldierConfirmed;
        [Foldout("Slider events")]
        public UnityEvent<float> OnSliderCanceled;

        public void SetupSlider(float initialValue)
        {
            Value = Mathf.Clamp01(initialValue);
            RefreshView();
        }

        public void Decrease() => ChangeValue(Value - step);

        public void Increase() => ChangeValue(Value + step);

        public void ConfirmSelection()
        {
            OnSldierConfirmed?.Invoke(Value);
            GoBack();
        }

        public void CancelSelection()
        {
            Value = entryValue;
            RefreshView();
            OnSliderCanceled?.Invoke(Value);
            GoBack();
        }

        protected override void OnActivate()
        {
            entryValue = Value;
            RefreshView();
            StartInput();
        }

        protected override void OnDeactivate()
        {
            StopInput();
        }

        protected override void OnDispose()
        {
            StopInput();
        }

        private void GoBack()
        {
            StopInput();
            Manager.PreviousBlock();
        }

        private void ChangeValue(float newValue)
        {
            var clampedValue = Mathf.Clamp01(newValue);
            if (Mathf.Approximately(clampedValue, Value))
                return;

            Value = clampedValue;
            RefreshView();
            OnSliderChanged?.Invoke(Value);
        }

        private void RefreshView()
        {
            if (barBack == null)
                return;

            var normalized = Mathf.Clamp01(Value);
            var trackWidth = GetTrackWidth();

            if (bar != null)
            {
                bar.anchorMin = new Vector2(barBack.anchorMin.x, barBack.anchorMin.y);
                bar.anchorMax = new Vector2(barBack.anchorMin.x, barBack.anchorMax.y);
                bar.pivot = new Vector2(0f, barBack.pivot.y);
                bar.anchoredPosition = barBack.anchoredPosition;
                bar.offsetMin = barBack.offsetMin;
                bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trackWidth * normalized);
            }

            if (cursor != null)
                SetCursorPosition(normalized);
        }

        private void SetCursorPosition(float normalized)
        {
            var corners = new Vector3[4];
            barBack.GetWorldCorners(corners);
            var targetWorld = Vector3.Lerp(corners[0], corners[3], normalized);
            var parent = (RectTransform)cursor.parent;
            var localPoint = parent.InverseTransformPoint(targetWorld);
            var anchorRef = new Vector2(
                parent.rect.xMin + parent.rect.width * cursor.anchorMin.x,
                parent.rect.yMin + parent.rect.height * cursor.anchorMin.y);

            cursor.anchoredPosition = new Vector2(localPoint.x - anchorRef.x, cursor.anchoredPosition.y);
        }

        private float GetTrackWidth()
        {
            var corners = new Vector3[4];
            barBack.GetWorldCorners(corners);
            var parent = (RectTransform)barBack.parent;
            var left = parent.InverseTransformPoint(corners[0]).x;
            var right = parent.InverseTransformPoint(corners[3]).x;
            return right - left;
        }

        private void StartInput()
        {
            StopInput();
            inputCoroutine = StartCoroutine(InputCoroutine());
        }

        private void StopInput()
        {
            if (inputCoroutine == null)
                return;

            StopCoroutine(inputCoroutine);
            inputCoroutine = null;
        }

        private IEnumerator InputCoroutine()
        {
            while (true)
            {
                yield return null;

                if (!IsActivated)
                    continue;

                if (Input.GetKeyDown(_options.MoveLeft))
                    Decrease();
                else if (Input.GetKeyDown(_options.MoveRight))
                    Increase();
                else if (Input.GetKeyDown(_options.Accept))
                    ConfirmSelection();
                else if (Input.GetKeyDown(_options.Cancel))
                    CancelSelection();
            }
        }
    }
}
