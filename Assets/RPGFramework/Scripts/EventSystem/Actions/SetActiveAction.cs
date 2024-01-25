using System.Collections;
using UnityEngine;

public class SetActiveAction : GraphActionBase
{
    public bool setActive;
    public GameObject gameObject;

    public SetActiveAction() : base("SetActive")
    {
        setActive = true;
        gameObject = null;
    }

    public override IEnumerator ActionCoroutine()
    {
        gameObject.SetActive(setActive);

        yield break;
    }

    public override string GetHeader()
    {
        return "Включить/Выключить объект";
    }
}