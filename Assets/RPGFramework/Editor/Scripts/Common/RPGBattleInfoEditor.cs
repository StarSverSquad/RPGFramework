using UnityEditor;
using UnityEngine;
using RPGF.RPG;

[CustomEditor(typeof(RPGBattleInfo))]
public class RPGBattleInfoEditor : Editor
{
    RPGBattleInfo info;

    private void OnEnable()
    {
        info = target as RPGBattleInfo;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("События битвы", GUI.skin.label);

        for (int i = 0; i < info.Events.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            info.Events[i].Period = (RPGBattleEvent.InvokePeriod)EditorGUILayout.EnumPopup("Период запуска", info.Events[i].Period);

            info.Events[i].IsCustomEvent = EditorGUILayout.Toggle("Самописное событие?", info.Events[i].IsCustomEvent);

            if (info.Events[i].IsCustomEvent)
                info.Events[i].CustomAction = (CustomActionBase)EditorGUILayout.ObjectField("Событие", info.Events[i].CustomAction, typeof(CustomActionBase), false);
            else
                info.Events[i].Event = (GraphEvent)EditorGUILayout.ObjectField("Событие", info.Events[i].Event, typeof(GraphEvent), false);

            if (info.Events[i].Period == RPGBattleEvent.InvokePeriod.OnPlayerTurn ||
                info.Events[i].Period == RPGBattleEvent.InvokePeriod.OnEnemyTurn)
            {
                GUILayout.Space(5);

                info.Events[i].Turn = EditorGUILayout.IntField("Ход", info.Events[i].Turn);
            }

            if (info.Events[i].Period == RPGBattleEvent.InvokePeriod.BeforeHit ||
                info.Events[i].Period == RPGBattleEvent.InvokePeriod.AfterHit)
            {
                GUILayout.Space(5);

                info.Events[i].EntityTag = EditorGUILayout.TextField("Тег", info.Events[i].EntityTag);
            }

            if (info.Events[i].Period == RPGBattleEvent.InvokePeriod.OnLessEnemyHeal ||
                info.Events[i].Period == RPGBattleEvent.InvokePeriod.OnLessCharacterHeal)
            {
                GUILayout.Space(5);

                info.Events[i].EntityTag = EditorGUILayout.TextField("Тег", info.Events[i].EntityTag);
                info.Events[i].Heal = EditorGUILayout.FloatField("Хп", info.Events[i].Heal);
            }

            if (GUILayout.Button("Удалить", GUILayout.Width(150)))
                info.Events.Remove(info.Events[i]);

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Добавить"))
            info.Events.Add(new RPGBattleEvent());

        EditorGUILayout.EndVertical();

        if (GUI.changed) 
            EditorUtility.SetDirty(info);
    }
}