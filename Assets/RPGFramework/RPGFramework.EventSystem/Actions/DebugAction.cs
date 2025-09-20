using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAction : GraphActionBase
{
    public enum WarningLevelType
    {
        Common, Warning, Error
    }

    public string ConsoleOutputText;

    public WarningLevelType WarningLevel;

    public DebugAction() : base("DebugAction")
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

    public override string GetHeader()
    {
        return "Debug";
    }
}
