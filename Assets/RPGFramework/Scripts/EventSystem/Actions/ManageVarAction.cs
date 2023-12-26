using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ManageVarAction : GraphActionBase
{
    public enum VarType
    {
        Bool, Int, Float, String
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
                if (!GameManager.Instance.GameData.BoolValues.HaveKey(VarName))
                    GameManager.Instance.GameData.BoolValues.Add(VarName, false);

                GameManager.Instance.GameData.BoolValues[VarName] = BoolBuffer;
                break;
            case VarType.String:
                if (!GameManager.Instance.GameData.StringValues.HaveKey(VarName))
                    GameManager.Instance.GameData.StringValues.Add(VarName, string.Empty);

                GameManager.Instance.GameData.StringValues[VarName] = StringBuffer;
                break;
            case VarType.Float:
                if (!GameManager.Instance.GameData.FloatValues.HaveKey(VarName))
                    GameManager.Instance.GameData.FloatValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameManager.Instance.GameData.FloatValues[VarName] = FloatBuffer;
                else
                    GameManager.Instance.GameData.FloatValues[VarName] += FloatBuffer;
                break;
            case VarType.Int:
                if (!GameManager.Instance.GameData.IntValues.HaveKey(VarName))
                    GameManager.Instance.GameData.IntValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameManager.Instance.GameData.IntValues[VarName] = IntBuffer;
                else
                    GameManager.Instance.GameData.IntValues[VarName] += IntBuffer;
                break;
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Управление переменной";
    }
}