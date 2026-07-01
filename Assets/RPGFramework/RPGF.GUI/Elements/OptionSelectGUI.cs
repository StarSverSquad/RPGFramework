using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI.Elements
{
    public class OptionSelectGUI : GUIBlock
    {
        [Inject]
        private readonly BaseOptions _options = null!;

        [SerializeField]
        private TextMeshProUGUI optionLabelText;
        [SerializeField]
        private RectTransform leftArrow;
        [SerializeField]
        private RectTransform rightArrow;
        [SerializeField]
        private bool wrapSelection;

        private readonly List<OptionSelectItem> options = new();
        private int entryIndex;
        private Coroutine inputCoroutine;

        public OptionSelectItem[] Options => options.ToArray();
        public int Index { get; private set; }
        public OptionSelectItem CurrentOption => HasOptions ? options[Index] : default;
        public bool HasOptions => options.Count > 0;

        [Foldout("OptionSelect events")]
        public UnityEvent<OptionSelectItem> OnOptionSelected;
        [Foldout("OptionSelect events")]
        public UnityEvent<OptionSelectItem> OnOptionSelectionChanged;
        [Foldout("OptionSelect events")]
        public UnityEvent<OptionSelectItem> OnOptionSelectionCanceled;

        public void SetupSelect(IEnumerable<OptionSelectItem> options, int initialIndex = 0)
        {
            this.options.Clear();
            if (options != null)
                this.options.AddRange(options);

            Index = ClampIndex(initialIndex);
            RefreshView();
        }

        public void SelectPrevious() => ChangeIndex(Index - 1);

        public void SelectNext() => ChangeIndex(Index + 1);

        public void ConfirmSelection()
        {
            if (!HasOptions)
                return;

            OnOptionSelected?.Invoke(CurrentOption);
            GoBack();
        }

        public void CancelSelection()
        {
            Index = entryIndex;
            RefreshView();
            OnOptionSelectionCanceled?.Invoke(HasOptions ? CurrentOption : default);
            GoBack();
        }

        protected override void OnActivate()
        {
            entryIndex = Index;
            RefreshView();
            StartInput();
        }

        protected override void OnDiativate()
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

        private void ChangeIndex(int newIndex)
        {
            if (!HasOptions)
                return;

            var clampedIndex = wrapSelection
                ? (newIndex % options.Count + options.Count) % options.Count
                : ClampIndex(newIndex);

            if (clampedIndex == Index)
                return;

            Index = clampedIndex;
            RefreshView();
            OnOptionSelectionChanged?.Invoke(CurrentOption);
        }

        private int ClampIndex(int value) => Mathf.Clamp(value, 0, Mathf.Max(0, options.Count - 1));

        private void RefreshView()
        {
            if (optionLabelText != null)
                optionLabelText.text = HasOptions ? CurrentOption.Label : string.Empty;

            if (leftArrow != null)
                leftArrow.gameObject.SetActive(HasOptions && options.Count > 1 && (wrapSelection || Index > 0));

            if (rightArrow != null)
                rightArrow.gameObject.SetActive(HasOptions && options.Count > 1 && (wrapSelection || Index < options.Count - 1));
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

                if (!IsActivated || !HasOptions)
                    continue;

                if (Input.GetKeyDown(_options.MoveLeft))
                    SelectPrevious();
                else if (Input.GetKeyDown(_options.MoveRight))
                    SelectNext();
                else if (Input.GetKeyDown(_options.Accept))
                    ConfirmSelection();
                else if (Input.GetKeyDown(_options.Cancel))
                    CancelSelection();
            }
        }
    }

    public struct OptionSelectItem
    {
        public string Label;
        public object Metadata;
    }
}
