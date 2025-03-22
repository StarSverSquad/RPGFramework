using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ManageVarAction : GraphActionBase
{
    public enum VarType
    {
        Bool, Int, Float, String, FastSave
    }

    public enum OperationType
    {
        Set, Add
    }

    public VarType Var;
    public OperationType Operation;

    public string VarName;

    public bool BoolBuffer;
    public int IntBuffer;
    public string StringBuffer;
    public float FloatBuffer;

    private SaveLoadManager SaveLoad => GameManager.Instance.SaveLoad;
    private GameData GameData => GameData;

    public ManageVarAction() : base("ManageVar")
    {
        Var = VarType.Bool;
        Operation = OperationType.Set;
        IntBuffer = 0;
        BoolBuffer = false;
        FloatBuffer = 0;
        StringBuffer = string.Empty;
        VarName = string.Empty;
    }

    public override IEnumerator ActionCoroutine()
    {
        switch (Var)
        {
            case VarType.Bool:
                if (!GameData.BoolValues.HaveKey(VarName))
                    GameData.BoolValues.Add(VarName, false);

                GameData.BoolValues[VarName] = BoolBuffer;
                break;
            case VarType.String:
                if (!GameData.StringValues.HaveKey(VarName))
                    GameData.StringValues.Add(VarName, string.Empty);

                GameData.StringValues[VarName] = StringBuffer;
                break;
            case VarType.Float:
                if (!GameData.FloatValues.HaveKey(VarName))
                    GameData.FloatValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameData.FloatValues[VarName] = FloatBuffer;
                else
                    GameData.FloatValues[VarName] += FloatBuffer;
                break;
            case VarType.Int:
                if (!GameData.IntValues.HaveKey(VarName))
                    GameData.IntValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameData.IntValues[VarName] = IntBuffer;
                else
                    GameData.IntValues[VarName] += IntBuffer;
                break;
            case VarType.FastSave:
                if (Operation == OperationType.Set)
                    SaveLoad.SetFastSave(VarName, IntBuffer);
                else
                    SaveLoad.SetFastSave(VarName, SaveLoad.GetFastSave(VarName) + IntBuffer);
                break;
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Управление переменной";
    }
}