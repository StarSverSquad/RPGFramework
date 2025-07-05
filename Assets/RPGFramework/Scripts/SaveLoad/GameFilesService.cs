using System.IO;
using UnityEngine;

namespace RPGF.SaveLoad
{
    public class GameFilesService
    {
        public const string SLOTFORMAT = "glackslot";
        public const string COMMONFORMAT = "glackcommon";
        public const string CONFIGFORMAT = "glackconfig";

        public const string COMMONNAME = "Common";
        public const string CONFIGNAME = "Config";

        public readonly DirectoryInfo SavePath;

        public GameFilesService()
        {
            SavePath = new(Application.dataPath + @"\Saves");

            if (!SavePath.Exists)
                SavePath.Create();
        }

        #region LOAD

        public object LoadFile(string filename)
        {
            try
            {
                string rawData = File.ReadAllText(Path.Combine(SavePath.FullName, filename));

                return JsonUtility.FromJson<object>(rawData);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }

        public T LoadFile<T>(string filename)
            where T : class
        {
            return (T)LoadFile(filename);
        }

        public GameSlotData LoadSlot(string slotName)
        {
            return LoadFile<GameSlotData>($"{slotName}.{SLOTFORMAT}");
        }

        public GameCommonData LoadCommon()
        {
            return LoadFile<GameCommonData>($"{COMMONNAME}.{COMMONFORMAT}");
        }

        public GameConfigData LoadConfig()
        {
            return LoadFile<GameConfigData>($"{CONFIGNAME}.{CONFIGFORMAT}");
        }

        #endregion

        #region SAVE

        public void SaveFile(string filename, object content)
        {
            File.WriteAllText(Path.Combine(SavePath.FullName, filename), JsonUtility.ToJson(content, true));
        }

        public void SaveFile<T>(string filename, T content)
        {
            SaveFile(filename, content as object);
        }

        public void SaveSlot(string slotName, GameSlotData data)
        {
            SaveFile($"{slotName}.{SLOTFORMAT}", data);
        }

        public void SaveCommon(GameCommonData data)
        {
            SaveFile($"{COMMONNAME}.{COMMONFORMAT}", data);
        }

        public void SaveConfig(GameConfigData data)
        {
            SaveFile($"{CONFIGNAME}.{CONFIGFORMAT}", data);
        }

        #endregion
    }
}
