using RPGF.Domain;
using System;

namespace RPGF.Core.SaveLoad
{
    [Serializable]
    public class GameCommonData
    {
        public CustomDictionary<int> FastSaves;

        public int LastLoadedSlotId;
        public int SlotsCount;

        public GameCommonData()
        {
            FastSaves = new CustomDictionary<int>();
        }
    }
}
