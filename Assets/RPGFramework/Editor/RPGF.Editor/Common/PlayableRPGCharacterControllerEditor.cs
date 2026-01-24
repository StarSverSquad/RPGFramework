using RPGF.Core.Character;
using RPGF.Editor.Core;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

namespace RPGF.Editor
{
    [CustomEditor(typeof(PlayableCharacterModelController))]
    public class PlayableRPGCharacterControllerEditor : RPGFrameworkEditor<PlayableCharacterModelController>
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                "Для корректной работы анимаций в Animator нужно указать следующие переменные:\n" +
                "Bool: IsMove, IsRun\n" +
                "Float: X, Y\n" +
                "Trigger: RESET",
                MessageType.Info);

            base.OnInspectorGUI();
        }
    }
}