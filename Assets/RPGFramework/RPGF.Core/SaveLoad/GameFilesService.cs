using System.Collections.Generic;
using System.IO;
using System.Linq;
using RPGF.Domain.DI;
using UnityEngine;

namespace RPGF.Core.SaveLoad
{
    public class GameFilesService : ISupportDI
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

        public T LoadFile<T>(string filename)
            where T : class
        {
            try
            {
                string rawData = File.ReadAllText(Path.Combine(SavePath.FullName, filename));

                return JsonUtility.FromJson<T>(rawData);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> LoadFiles<T>(string fileFormat)
            where T : class
        {
            try
            {
                var files = Directory.GetFiles(SavePath.FullName).Where(file => file.EndsWith(fileFormat));

                return files.Select(file => LoadFile<T>(file));
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        public GameSlotData LoadSlot(string slotName)
        {
            return LoadFile<GameSlotData>($"{slotName}.{SLOTFORMAT}");
        }

        public IEnumerable<GameSlotData> LoadAllSlots()
        {
            return LoadFiles<GameSlotData>(SLOTFORMAT);
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
