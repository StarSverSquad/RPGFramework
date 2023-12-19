using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAction : GraphActionBase
{
    [SerializeReference]
    public List<string> Choices;

    public ChoiceBoxManager.Position Position;

    public Vector2 CustomPosition;

    public ChoiceAction() : base("Choice")
    {
        Choices = new List<string>();
        CustomPosition = new Vector2();
        Position = ChoiceBoxManager.Position.Bottom;
    }

    public override IEnumerator ActionCoroutine()
    {
        CommonManager.instance.choiceBox.ChangePosition(Position, CustomPosition);

        CommonManager.instance.choiceBox.Choice(Choices.ToArray());

        yield return new WaitWhile(() => CommonManager.instance.choiceBox.IsChoicing);

        nextIndex = CommonManager.instance.choiceBox.Index;
    }

    public override string GetHeader()
    {
        return "Выбор";
    }
}