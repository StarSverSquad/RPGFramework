using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EffectEditorService
{
    public void BuildGUI(EffectBase effect)
    {
        switch (effect)
        {
            case ChangeManaHealConstEffect cmhc:
                {
                    cmhc.Heal = EditorGUILayout.IntField("HEAL", cmhc.Heal);
                    cmhc.Mana = EditorGUILayout.IntField("MANA", cmhc.Mana);
                }
                break;
            case ChangeManaHealPercentEffect cmhp:
                {
                    cmhp.Heal = EditorGUILayout.FloatField("HEAL%", cmhp.Heal);
                    cmhp.Mana = EditorGUILayout.FloatField("MANA%", cmhp.Mana);
                }
                break;
            case InvokeEventEffect ie:
                {
                    ie.@event = (GraphEvent)EditorGUILayout.ObjectField("Событие", ie.@event, typeof(GraphEvent), false);
                }
                break;
            case ChangeConcentrationEffect cc:
                {
                    cc.AddConcentration = EditorGUILayout.IntField("Концентрация", cc.AddConcentration);
                }
                break;
            case ChangeStateEffect cs:
                {
                    cs.IsAddState = EditorGUILayout.Toggle("Добавить?", cs.IsAddState);
                    cs.State = (RPGEntityState)EditorGUILayout.ObjectField("Событие", cs.State, typeof(RPGEntityState), false);
                }
                break;
            case RemoveAllStatesEffect:
                EditorGUILayout.HelpBox("Удалит все состояния", MessageType.Info);
                break;
            default:
                EditorGUILayout.HelpBox("Для эффекта не создан интерфейс!", MessageType.Warning);
                break;
        }
    }
}