using UnityEditor;
using UnityEngine;
using RPGF.RPG;
using RPGF.Editor.Core;

namespace RPGF.Editor
{
    [CustomEditor(typeof(RPGBattleInfo))]
    public class RPGBattleInfoEditor : RPGFrameworkEditor<RPGBattleInfo>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical(UnityEngine.GUI.skin.box);

            EditorGUILayout.LabelField("События битвы", UnityEngine.GUI.skin.label);

            for (int i = 0; i < Target.Events.Count; i++)
            {
                EditorGUILayout.BeginVertical(UnityEngine.GUI.skin.box);

                Target.Events[i].Period = (RPGBattleEvent.InvokePeriod)EditorGUILayout.EnumPopup("Период запуска", Target.Events[i].Period);

                Target.Events[i].IsCustomEvent = Toggle("Самописное событие?", Target.Events[i].IsCustomEvent);

                if (Target.Events[i].IsCustomEvent)
                    Target.Events[i].CustomAction = ObjectField("Событие", Target.Events[i].CustomAction);
                else
                    Target.Events[i].Event = ObjectField("Событие", Target.Events[i].Event);

                if (Target.Events[i].Period == RPGBattleEvent.InvokePeriod.OnPlayerTurn ||
                    Target.Events[i].Period == RPGBattleEvent.InvokePeriod.OnEnemyTurn)
                {
                    GUILayout.Space(5);

                    Target.Events[i].Turn = EditorGUILayout.IntField("Ход", Target.Events[i].Turn);
                }

                if (Target.Events[i].Period == RPGBattleEvent.InvokePeriod.BeforeHit ||
                    Target.Events[i].Period == RPGBattleEvent.InvokePeriod.AfterHit)
                {
                    GUILayout.Space(5);

                    Target.Events[i].EntityTag = TextField("Тег", Target.Events[i].EntityTag);
                }

                if (Target.Events[i].Period == RPGBattleEvent.InvokePeriod.OnLessEnemyHeal ||
                    Target.Events[i].Period == RPGBattleEvent.InvokePeriod.OnLessCharacterHeal)
                {
                    GUILayout.Space(5);

                    Target.Events[i].EntityTag = TextField("Тег", Target.Events[i].EntityTag);
                    Target.Events[i].Heal = EditorGUILayout.FloatField("Хп", Target.Events[i].Heal);
                }

                if (GUILayout.Button("Удалить", GUILayout.Width(150)))
                    Target.Events.Remove(Target.Events[i]);

                EditorGUILayout.EndVertical();
            }

            if (Button("Добавить"))
                Target.Events.Add(new RPGBattleEvent());

            EditorGUILayout.EndVertical();

            if (GuiChanged)
                EditorUtility.SetDirty(Target);
        }
    }
}