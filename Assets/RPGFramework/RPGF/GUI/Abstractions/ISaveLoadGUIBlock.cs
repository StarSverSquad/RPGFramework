using RPGF.GUI.Interfaces;

namespace RPGF.GUI.Abstractions
{
    public interface ISaveLoadGUIBlock : IGUIBlock
    {
        public bool IsSaveMode { get; }

        public void SetSaveMode(bool saveMode);
    }
}