using RPGF.Core;
using RPGF.Core.Choice;
using RPGF.Domain.DI;
using System;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.Choice
{
    public class BattleChoice : PaginationChoiceBase<BattleChoiceItem>, IDisposable
    {
        [Inject]
        private readonly BaseOptions options;

        [SerializeField]
        private BattleChoiceItemUI[] ItemUIs = new BattleChoiceItemUI[8];
        [SerializeField]
        private int lineSize = 2;

        public event Action<int> OnConfirmLockedEvent;

        protected override void OnStarted()
        {
            UpdateUI();

            Items[Index].Element.Focus();
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

            Items[Index].Element.Focus();
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
                ui.Icon.sprite = item.Icon;

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
                Destroy(item.gameObject);
            }
            ItemUIs = null;
        }
    }
}
