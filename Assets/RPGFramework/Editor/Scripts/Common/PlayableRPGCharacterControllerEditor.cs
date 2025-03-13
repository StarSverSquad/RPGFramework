

using RPGF.Character;
using UnityEditor;

[CustomEditor(typeof(PlayableRPGCharacterController))]
class PlayableRPGCharacterControllerEditor : Editor
{
    private PlayableRPGCharacterController characterController;

    private void OnEnable()
    {
        characterController = (PlayableRPGCharacterController)target;
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