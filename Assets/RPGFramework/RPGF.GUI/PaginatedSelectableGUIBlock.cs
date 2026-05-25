using System.Collections.Generic;
using UnityEngine;

namespace RPGF.GUI
{
    public abstract class PaginatedSelectableGUIBlock<T> : GUISelectableBlock
    {
        protected List<T> SelectedItems { get; } = new();

        public int Page { get; private set; }
        public int ItemCount => SelectedItems.Count;
        public int MaxPage => CalculateMaxPage();

        public int PageSize => Elements.Count;

        public bool HasItems => SelectedItems.Count > 0;

        protected int AbsoluteIndex => CurrentIndex + (Page * PageSize);

        protected int CalculateMaxPage()
        {
            if (ItemCount == 0 || PageSize == 0)
            {
                return 0;
            }

            return Mathf.CeilToInt((float)ItemCount / PageSize) - 1;
        }

        protected int ToAbsoluteIndex(int pageIndex) => pageIndex + (Page * PageSize);

        protected T GetItemAt(int absoluteIndex) => SelectedItems[absoluteIndex];

        protected T GetCurrentItem() => SelectedItems[AbsoluteIndex];

        public void RefreshItems()
        {
            RepopulateItems();
            OnItemsRefreshed();
            SetPage(0);
        }

        protected virtual void RepopulateItems()
        {
            SelectedItems.Clear();
            SelectedItems.AddRange(BuildItems());
        }

        protected abstract IEnumerable<T> BuildItems();

        protected void SetPage(int page)
        {
            Page = page;

            for (int i = 0; i < PageSize; i++)
            {
                var itemIndex = i + (page * PageSize);
                if (itemIndex >= SelectedItems.Count)
                {
                    HideElement(i);
                    continue;
                }

                BindElement(i, SelectedItems[itemIndex]);
            }

            UpdatePaginationArrows(page, MaxPage);
        }

        protected override void ChangeSelect(int newIndex)
        {
            if (PageSize == 0 || !HasItems)
            {
                return;
            }

            if (Page > 0 && newIndex < 0)
            {
                SetPage(Page - 1);
                base.ChangeSelect(PageSize - 1);
            }
            else if (Page < MaxPage && newIndex >= PageSize)
            {
                SetPage(Page + 1);
                base.ChangeSelect(0);
            }
            else
            {
                var absoluteIndex = ToAbsoluteIndex(newIndex);
                if (absoluteIndex >= SelectedItems.Count || absoluteIndex < 0)
                {
                    return;
                }

                base.ChangeSelect(newIndex);
            }

            OnItemSelected(GetCurrentItem());
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            Page = 0;
            SelectedItems.Clear();
        }

        protected abstract void BindElement(int elementIndex, T item);

        protected abstract void HideElement(int elementIndex);

        protected abstract void UpdatePaginationArrows(int page, int maxPage);

        protected virtual void OnItemsRefreshed() { }

        protected virtual void OnItemSelected(T item) { }

        protected void ShowElement(int elementIndex)
        {
            Elements[elementIndex].gameObject.SetActive(true);
        }

        protected void HideElementAt(int elementIndex)
        {
            Elements[elementIndex].gameObject.SetActive(false);
        }
    }
}
