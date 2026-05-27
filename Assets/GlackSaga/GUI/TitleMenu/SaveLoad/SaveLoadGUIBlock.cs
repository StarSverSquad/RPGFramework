using System.Collections.Generic;
using System.Linq;
using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Abstractions;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.SaveLoad
{
    public class SaveLoadGUIBlock : PaginatedSelectableGUIBlock<GameSlotData>, ISaveLoadGUIBlock
    {
        [Inject]
        private readonly SaveLoadService _saveLoad = null!;
        [Inject]
        private readonly GameFilesService _gameFiles = null!;
        [Inject]
        private readonly GameCommonDataService _commonData = null!;

        [SerializeField]
        private GameObject arrowUp;
        [SerializeField]
        private GameObject arrowDown;
        [SerializeField]
        private AudioSource LoadSound;
        [SerializeField]
        private AudioSource SaveSound;
        [SerializeField]
        private GameObject noSavesText;
        [SerializeField]
        private GameObject saveModeLabel;

        public bool IsSaveMode { get; private set; }

        public void SetSaveMode(bool saveMode) => IsSaveMode = saveMode;

        protected override void OnFocus()
        {
            saveModeLabel.SetActive(IsSaveMode);

            RefreshItems();

            if (HasItems)
            {
                StartChoice();
            }
        }

        protected override void OnChoiced(int index)
        {
            var slot = GetItemAt(ToAbsoluteIndex(index));

            if (IsSaveMode)
            {
                SaveSlot(slot);
            }
            else
            {
                LoadSlot(slot);
            }
        }

        protected override void BindElement(int elementIndex, GameSlotData item)
        {
            if (Elements[elementIndex] is SaveSlotItem slotItem)
            {
                if (item == null)
                {
                    slotItem.SetNewSlot();
                }
                else
                {
                    slotItem.SetData(item);
                }
            }
        }

        protected override IEnumerable<GameSlotData> BuildItems()
        {
            var slots = _gameFiles.LoadAllSlots()?
                .Where(s => s != null)
                .OrderBy(s => s.Id)
                .ToList() ?? new List<GameSlotData>();

            if (IsSaveMode)
            {
                slots.Add(null);
            }

            noSavesText.SetActive(slots.Count == 0);

            return slots;
        }

        protected override void UpdatePaginationArrows(int page, int maxPage)
        {
            arrowUp.SetActive(page > 0);
            arrowDown.SetActive(page < maxPage);
        }

        private void SaveSlot(GameSlotData slot)
        {
            int slotId = slot?.Id ?? GetNewSlotId();

            _saveLoad.GameSave(slotId);
            SaveSound.Play();

            if (slot == null)
            {
                _commonData.CommonData.SlotsCount = Mathf.Max(_commonData.CommonData.SlotsCount, slotId + 1);
                _commonData.Save();
            }

            Preview();
        }

        private void LoadSlot(GameSlotData slot)
        {
            if (slot == null)
            {
                return;
            }

            _saveLoad.GameLoad(slot.Id);
            LoadSound.Play();

            _commonData.CommonData.LastLoadedSlotId = slot.Id;
            _commonData.Save();

            Manager.Close();
        }

        private int GetNewSlotId()
        {
            var slots = _gameFiles.LoadAllSlots()?.Where(s => s != null);

            if (slots == null || !slots.Any())
            {
                return 1;
            }

            return slots.Max(s => s.Id) + 1;
        }
    }
}
