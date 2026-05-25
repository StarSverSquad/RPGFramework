using RPGF;
using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using System;
using System.Collections;

namespace RPGF.Actions
{
    [Serializable]
    public class ManageVarAction : ActionBase
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

        [Inject]
        private readonly FastSaveService _fastSave = null!;
        [Inject]
        private readonly GameData _gameData = null!;

        public string VarName;

        public bool BoolBuffer;
        public int IntBuffer;
        public string StringBuffer;
        public float FloatBuffer;

        public ManageVarAction() : base()
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
                    if (!_gameData.BoolValues.HaveKey(VarName))
                        _gameData.BoolValues.Add(VarName, false);

                    _gameData.BoolValues[VarName] = BoolBuffer;
                    break;
                case VarType.String:
                    if (!_gameData.StringValues.HaveKey(VarName))
                        _gameData.StringValues.Add(VarName, string.Empty);

                    _gameData.StringValues[VarName] = StringBuffer;
                    break;
                case VarType.Float:
                    if (!_gameData.FloatValues.HaveKey(VarName))
                        _gameData.FloatValues.Add(VarName, 0);

                    if (Operation == OperationType.Set)
                        _gameData.FloatValues[VarName] = FloatBuffer;
                    else
                        _gameData.FloatValues[VarName] += FloatBuffer;
                    break;
                case VarType.Int:
                    if (!_gameData.IntValues.HaveKey(VarName))
                        _gameData.IntValues.Add(VarName, 0);

                    if (Operation == OperationType.Set)
                        _gameData.IntValues[VarName] = IntBuffer;
                    else
                        _gameData.IntValues[VarName] += IntBuffer;
                    break;
                case VarType.FastSave:
                    if (Operation == OperationType.Set)
                        _fastSave[VarName] = IntBuffer;
                    else
                        _fastSave[VarName] += IntBuffer;
                    break;
            }

            yield break;
        }
    }
}
