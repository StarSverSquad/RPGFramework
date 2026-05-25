using System.Collections.Generic;
using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using RPGF.GUI;

namespace GlackSaga.GUI.TitleMenu.SaveLoad
{
    public class SaveLoadGUIBlock : PaginatedSelectableGUIBlock<GameSlotData>
    {
        [Inject]
        private readonly SaveLoadService _saveLoadService = null!;

        public bool IsSaveMode { get; private set; }

        protected override void BindElement(int elementIndex, GameSlotData item)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<GameSlotData> BuildItems()
        {
            throw new System.NotImplementedException();
        }

        protected override void HideElement(int elementIndex)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdatePaginationArrows(int page, int maxPage)
        {
            throw new System.NotImplementedException();
        }
    }
}