using RPGF.EventSystem.Attributes;
using System.Collections;
using UnityEngine;

namespace RPGF.EventSystem.Default
{
    [GenerateActionNode("Отладка")]
    public class DebugAction : ActionBase
    {
        public enum WarningLevelType
        {
            Common, Warning, Error
        }

        [ActionFieldOption("Уровень предупреждения:")]
        public WarningLevelType WarningLevel;

        [ActionFieldOption("Текст", MultiLine = true)]
        public string ConsoleOutputText;

        public DebugAction() : base()
        {
            WarningLevel = WarningLevelType.Common;
            ConsoleOutputText = string.Empty;
        }

        public override IEnumerator ActionCoroutine()
        {
            switch (WarningLevel)
            {
                case WarningLevelType.Common:
                    Debug.Log(ConsoleOutputText);
                    break;
                case WarningLevelType.Warning:
                    Debug.LogWarning(ConsoleOutputText);
                    break;
                case WarningLevelType.Error:
                    Debug.LogError(ConsoleOutputText);
                    break;
            }

            yield break;
        }
    }
}