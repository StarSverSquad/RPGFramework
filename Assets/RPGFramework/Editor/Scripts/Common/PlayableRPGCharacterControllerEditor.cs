

using RPGF.Character;
using UnityEditor;

[CustomEditor(typeof(PlayableCharacterModelController))]
class PlayableRPGCharacterControllerEditor : Editor
{
    private PlayableCharacterModelController characterController;

    private void OnEnable()
    {
        characterController = (PlayableCharacterModelController)target;
    }

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