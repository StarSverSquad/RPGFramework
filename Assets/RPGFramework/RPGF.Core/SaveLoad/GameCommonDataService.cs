using RPGF.Domain;
using RPGF.Domain.DI;

namespace RPGF.Core.SaveLoad
{
    public class GameCommonDataService : ISupportDI
    {
        [Inject]
        private readonly GameFilesService _gameFiles;

        public GameCommonData CommonData { get; private set; }

        public void Load()
        {
            CommonData = _gameFiles.LoadCommon();

            if (CommonData == null)
            {
                CommonData = CreateNew();
                Save();
            }
        }

        public void Save()
        {
            _gameFiles.SaveCommon(CommonData);
        }

        public GameCommonData CreateNew()
        {
            return new GameCommonData()
            {
                FastSaves = new CustomDictionary<int>(),
                LastLoadedSlotId = -1,
                SlotsCount = 0
            };
        }
    }
}
