using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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