using RPGF.Core;
using RPGF.Core.Battle;
using RPGF.Core.Choice;
using RPGF.Domain.DI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RPGF.Battle.Choice
{
    public class BattleChoice : PaginationChoiceBase<BattleChoiceItem>, IDisposable
    {
        [Inject]
        private readonly BaseOptions options;
        [Inject]
        private readonly BattleConfig _config;
        [Inject]
        private readonly BattleAudioManager _audio;


        [SerializeField]
        private RectTransform pagesContainer;
        [SerializeField]
        private RectTransform[] pageItems = new RectTransform[10];
        [SerializeField]
        private GameObject paginationObject;
        [SerializeField]
        private GameObject paginationUpArrowObject;
        [SerializeField]
        private GameObject paginationDownArrowObject;

        [SerializeField]
        private BattleChoiceItemUI[] ItemUIs = new BattleChoiceItemUI[8];
        [SerializeField]
        private int lineSize = 2;

        public event Action<int> OnConfirmLockedEvent;

        protected override void OnStarted()
        {
            UpdateUI();

            Items[Index].Element.Focus();

            InitializePaginationUI();
        }

        protected override void OnSelectionChanged(BattleChoiceItem item, int index, int prevIndex)
        {
            base.OnSelectionChanged(item, index, prevIndex);

            Items[prevIndex].Element.UnFocus();
            Items[Index].Element.Focus();
        }

        protected override void OnPageChanged(int newPage, int oldPage)
        {
            UpdateUI();

            if (PageCount > 1)
            {
                UpdatePaginationUI();
            }

            Items[Index].Element.Focus();
        }

        protected override void OnCanceled()
        {
            base.OnCanceled();

            _audio.PlaySound(_config.CancelSound);
        }

        protected override void OnConfirmed(BattleChoiceItem resultItem)
        {
            base.OnConfirmed(resultItem);

            _audio.PlaySound(_config.SellectSound);
        } 

        private void UpdateUI()
        {
            int indexUI = 0;
            foreach (var item in Items.Skip(PageSize * Page).Take(PageSize))
            {
                if (indexUI >= ItemUIs.Length)
                    break;

                var ui = ItemUIs[indexUI];

                item.Element = ui;

                ui.MainText.text = item.Label;
                ui.CounterText.text = item.CounterText;

                item.Element.SetIcon(item.Icon);

                ui.UnFocus();

                if (item.IsLocked)
                    ui.Lock();
                else
                    ui.UnLock();

                ui.gameObject.SetActive(true);

                indexUI++;
            }

            for ( ; indexUI < ItemUIs.Length; indexUI++)
            {
                var ui = ItemUIs[indexUI];

                ui.gameObject.SetActive(false);
            }
        }


        private void InitializePaginationUI()
        {
            if (PageCount > 1)
            {
                var pagesContainerSize = pagesContainer.sizeDelta;
                pagesContainerSize.y = (pageItems.First().sizeDelta.y + 5.5f) * PageCount;
                pagesContainer.sizeDelta = pagesContainerSize;

                pagesContainer.anchoredPosition = new Vector2(-4, 0);

                paginationObject.SetActive(true);

                UpdatePaginationUI();
            }
            else
            {
                paginationObject.SetActive(false);
            }
        }

        private void UpdatePaginationUI()
        {
            paginationDownArrowObject.SetActive(false);
            paginationUpArrowObject.SetActive(false);

            if (Page == 0)
            {
                paginationDownArrowObject.SetActive(true);
            }
            else if (Page == PageCount - 1)
            {
                paginationUpArrowObject.SetActive(true);
            }
            else
            {
                paginationDownArrowObject.SetActive(true);
                paginationUpArrowObject.SetActive(true);
            }

            for (int i = 0; i < pageItems.Length; i++)
            {
                if (i < PageCount)
                    pageItems[i].gameObject.SetActive(true);
                else
                    pageItems[i].gameObject.SetActive(false);

                var subImage = pageItems[i].GetChild(0).GetComponent<Image>();

                if (Page == i)
                {
                    subImage.enabled = true;
                }
                else
                {
                    subImage.enabled = false;
                }
            }
        }

        #region control

        protected override bool CancelCanExecute()
        {
            return Input.GetKeyDown(options.Cancel);
        }

        protected override bool ConfirmCanExecute()
        {
            if (Input.GetKeyDown(options.Accept))
            {
                if (Current.IsLocked)
                {
                    OnConfirmLockedEvent?.Invoke(Index);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        protected override int SelectionChange(int currentIndex)
        {
            if (Input.GetKeyDown(options.MoveDown))
            {
                return currentIndex + lineSize;
            }
            else if (Input.GetKeyDown(options.MoveUp))
            {
                return currentIndex - lineSize;
            }
            else if (Input.GetKeyDown(options.MoveRight))
            {
                return currentIndex + 1;
            }
            else if (Input.GetKeyDown(options.MoveLeft))
            {
                return currentIndex - 1;
            }

            return currentIndex;
        }

        #endregion

        public void Dispose() 
        {
            foreach (var item in ItemUIs)
            {
                item.gameObject.SetActive(false);
            }

            paginationObject.SetActive(false);
        }
    }
}
