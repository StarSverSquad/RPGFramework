

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
            "Для корректной работы анимаций в Animator нужно что были следующие переменные:\n" +
            "Bool: IsMove, IsRun\n" +
            "Int: X, Y", 
            MessageType.Info);

        base.OnInspectorGUI();
    }
}