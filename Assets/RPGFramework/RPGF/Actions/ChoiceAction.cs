using RPGF.EventSystem;
using RPGF.Shared;
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
        SharedManager.Instance.ChoiceBox.ChangePosition(Position, CustomPosition);

        SharedManager.Instance.ChoiceBox.Choice(Choices.ToArray());

        yield return new WaitWhile(() => SharedManager.Instance.ChoiceBox.IsChoicing);

        nextIndex = SharedManager.Instance.ChoiceBox.Index;
    }

    public override string GetHeader()
    {
        return "Выбор";
    }
}