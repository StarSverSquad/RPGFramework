using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Choice
{
    public enum ChoiceState
    {
        Pending, Chocing, Confirmed, Canceled
    }

    public abstract class ChoiceBase<T> : RPGFrameworkBehaviour
        where T : ChoiceItem
    {
        public List<T> Items { get; private set; }
        public int Index { get; private set; }

        public T ResultItem { get; private set; }
        public T Current => Items[Index];

        public ChoiceState State { get; private set; }

        protected Coroutine choiceCoroutine = null;
        public bool IsChoiceCoroutineWorking => choiceCoroutine is not null;

        public event Action<T, int, int> OnSelectionChangedEvent;
        public event Action<T> OnConfirmEvent;
        public event Action OnCancelEvent;
        public event Action OnEndedEvent;
        public event Action OnStartedEvent;

        public override void Initialize()
        {
            ResetChoice();
        }

        public Coroutine Invoke(IEnumerable<T> items = null, int startIndex = 0)
        {
            if (IsChoiceCoroutineWorking)
            {
                Debug.LogWarning("Выбор уже запущен! (Выбор будет перезапущен)");
                StopCoroutine(choiceCoroutine);
            }

            Index = startIndex;

            Items = items?.ToList() ?? Items;

            return choiceCoroutine = StartCoroutine(ChoiceCoroutine());
        }

        public void ResetChoice()
        {
            if (IsChoiceCoroutineWorking)
            {
                StopCoroutine(choiceCoroutine);
            }

            Index = 0;

            ResultItem = null;

            State = ChoiceState.Pending;
        }

        public void ForceSelectionChange(int newIndex)
        {
            int prevIndex = Index;

            Index = Mathf.Clamp(newIndex, 0, Items.Count - 1); ;

            OnSelectionChanged(Current, Index, prevIndex);
            OnSelectionChangedEvent?.Invoke(Current, Index, prevIndex);
        }

        public void ForceConfirm()
        {
            State = ChoiceState.Confirmed;
        }
        public void ForceCancel()
        {
            State = ChoiceState.Canceled;
        }

        #region Virtuals

        protected virtual void OnSelectionChanged(T item, int index, int prevIndex) { }

        protected virtual void OnConfirmed(T resultItem) { }
        protected virtual void OnCanceled() { }
        protected virtual void OnEnded() { }
        protected virtual void OnStarted() { }

        #endregion

        #region Abstrations

        protected abstract int SelectionChange(int currentIndex);

        protected abstract bool ConfirmCanExecute();
        protected abstract bool CancelCanExecute();

        #endregion

        private IEnumerator ChoiceCoroutine()
        {
            State = ChoiceState.Chocing;

            OnStarted();
            OnStartedEvent?.Invoke();

            yield return null;

            OnSelectionChanged(Current, Index, Index);
            OnSelectionChangedEvent?.Invoke(Current, Index, Index);

            while (State == ChoiceState.Chocing)
            {
                int newIndex = SelectionChange(Index);

                if (newIndex != Index)
                {
                    int prevIndex = Index;

                    Index = Mathf.Clamp(newIndex, 0, Items.Count - 1);
                    OnSelectionChanged(Current, Index, prevIndex);
                    OnSelectionChangedEvent?.Invoke(Current, Index, prevIndex);
                }

                if (ConfirmCanExecute())
                {
                    State = ChoiceState.Confirmed;
                }

                if (CancelCanExecute())
                {
                    State = ChoiceState.Canceled;
                }

                yield return null;
            }

            if (State == ChoiceState.Confirmed)
            {
                ResultItem = Current;

                OnConfirmed(ResultItem);
                OnConfirmEvent?.Invoke(ResultItem);
            }
            else if (State == ChoiceState.Canceled)
            {
                OnCanceled();
                OnCancelEvent?.Invoke();
            }

            OnEnded();
            OnEndedEvent?.Invoke();

            choiceCoroutine = null;
        }
    }
}