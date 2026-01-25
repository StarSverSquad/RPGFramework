using System;
using UnityEngine;

namespace RPGF.Core.Choice
{
    public abstract class PaginationChoiceBase<T> : ChoiceBase<T>
        where T : ChoiceItem
    {
        [SerializeField]
        private int PageSize = 10;

        public int Page => Mathf.FloorToInt((Index + 1) / PageSize);

        public event Action<int, int> OnPageChangedEvent;

        protected override void OnSelectionChanged(T item, int index, int prevIndex)
        {
            base.OnSelectionChanged(item, index, prevIndex);

            int newPage = Mathf.FloorToInt((index + 1) / PageSize);
            int oldPage = Mathf.FloorToInt((prevIndex + 1) / PageSize);

            if (oldPage != newPage)
            {
                OnPageChanged(newPage, oldPage);
                OnPageChangedEvent?.Invoke(newPage, oldPage);
            }
        }

        #region VIRTUALS

        protected virtual void OnPageChanged(int newPage, int oldPage) { }

        #endregion
    }
}
