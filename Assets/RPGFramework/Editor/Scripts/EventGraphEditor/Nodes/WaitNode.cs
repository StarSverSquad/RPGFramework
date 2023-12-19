using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class WaitNode : ActionNodeBase
{
    public WaitNode(WaitAction action) : base(action)
    {
    }

    public override void UIContructor()
    {
        WaitAction wait = action as WaitAction;

        FloatField timeField = new FloatField("Время");

        timeField.SetValueWithoutNotify(wait.time);
        timeField.RegisterValueChangedCallback(time =>
        {
            wait.time = time.newValue;

            MakeDirty();
        });

        extensionContainer.Add(timeField);
    }
}
