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
                if (!GameManager.Instance.gameData.BoolValues.HaveKey(VarName))
                    GameManager.Instance.gameData.BoolValues.Add(VarName, false);

                GameManager.Instance.gameData.BoolValues[VarName] = BoolBuffer;
                break;
            case VarType.String:
                if (!GameManager.Instance.gameData.StringValues.HaveKey(VarName))
                    GameManager.Instance.gameData.StringValues.Add(VarName, string.Empty);

                GameManager.Instance.gameData.StringValues[VarName] = StringBuffer;
                break;
            case VarType.Float:
                if (!GameManager.Instance.gameData.FloatValues.HaveKey(VarName))
                    GameManager.Instance.gameData.FloatValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameManager.Instance.gameData.FloatValues[VarName] = FloatBuffer;
                else
                    GameManager.Instance.gameData.FloatValues[VarName] += FloatBuffer;
                break;
            case VarType.Int:
                if (!GameManager.Instance.gameData.IntValues.HaveKey(VarName))
                    GameManager.Instance.gameData.IntValues.Add(VarName, 0);

                if (Operation == OperationType.Set)
                    GameManager.Instance.gameData.IntValues[VarName] = IntBuffer;
                else
                    GameManager.Instance.gameData.IntValues[VarName] += IntBuffer;
                break;
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Управление переменной";
    }
}