using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveNode : ActionNodeBase
{
    public SaveNode(SaveAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        SaveAction se = action as SaveAction;

        IntegerField SaveField = new IntegerField("Ячейка для сохранения");

        SaveField.SetValueWithoutNotify(se.slotId);
        SaveField.RegisterValueChangedCallback(vol =>
        {
            se.slotId = (vol.newValue <= 0) ? 0 : vol.newValue;

            MakeDirty();
        });

        extensionContainer.Add(SaveField);
    }

}
