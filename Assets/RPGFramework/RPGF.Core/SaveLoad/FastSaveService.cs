namespace RPGF.SaveLoad
{
    public class FastSaveService
    {
        private readonly GameCommonDataService _commonData;

        public int this[string key]
        {
            get => GetKey(key);
            set => SetKey(key, value);
        }

        public FastSaveService(GameCommonDataService commonData)
        {
            _commonData = commonData;
        }

        public bool HaveKey(string key)
        {
            return _commonData.CommonData.FastSaves.HaveKey(key);
        }

        public void SetKey(string key, int value)
        {
            if (!HaveKey(key))
                _commonData.CommonData.FastSaves.Add(key, value);
            else
                _commonData.CommonData.FastSaves.Set(key, value);
        }

        public int GetKey(string key)
        {
            return _commonData.CommonData.FastSaves.Get(key);
        }
    }
}
