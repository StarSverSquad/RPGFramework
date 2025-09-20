using RPGF.EventSystem;
using System.Collections;

public class InvokeCustomAction : GraphActionBase
{
    public CustomActionBase act;

    public InvokeCustomAction() : base("ICA")
    {
    }

    public override IEnumerator ActionCoroutine()
    {
        yield return act.Invoke();
    }

    public override string GetHeader()
    {
        return "Zaпуск самопистного события";
    }
}