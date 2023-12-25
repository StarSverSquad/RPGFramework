using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameUIManager : ContentManagerBase
{
    public TextMeshProUGUI txt;

    public override void InitializeChild()
    {
        
    }

    // ��� �Ѩ ����� ���� �� �������

    private void Start()
    {
        
    }

#if UNITY_EDITOR
    private void FixedUpdate()
    {
        txt.text = "{";

        foreach (InventorySlot slot in GameManager.Instance.inventory)
        {
            txt.text += $"{slot.Item.Name} : {slot.Count}}}, {{";
        }

        txt.text += "}\n{";

        foreach (var character in GameManager.Instance.Character.characters)
        {
            txt.text += $"{character.Name}, HEAL:{character.Heal}, MANA:{character.Mana}" +
                $", DMG:{character.Damage}, DEF:{character.Defence}" +
                $", AGI:{character.Agility}, LUCK:{character.Luck},\n[";

            foreach (var state in character.States)
            {
                txt.text += $"{state.Name},";
            }

            txt.text += "]}, \n{";
        }

        txt.text += "}";
    }
#endif
}
