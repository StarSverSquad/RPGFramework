using System;
using UnityEngine;

namespace RPGF.Core.Choice
{
    public abstract class PaginationChoiceBase<T> : ChoiceBase<T>
        where T : ChoiceItem
    {
        [SerializeField]
        private int pageSize = 10;
        public int PageSize => pageSize;

        public int Page => Mathf.FloorToInt((float)Index / (float)pageSize);
        public int PageCount => Mathf.CeilToInt((float)Items.Count / (float)pageSize);

        public event Action<int, int> OnPageChangedEvent;

        protected override void OnSelectionChanged(T item, int index, int prevIndex)
        {
            base.OnSelectionChanged(item, index, prevIndex);

            int newPage = Mathf.FloorToInt((float)index / (float)pageSize);
            int oldPage = Mathf.FloorToInt((float)prevIndex / (float)pageSize);

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
