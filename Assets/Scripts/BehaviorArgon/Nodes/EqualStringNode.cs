using System.Collections;
using UnityEngine;

public class EqualStringNode : Condition
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A", StringTypePS, vs);
        AddParameter(1, "B", StringTypePS, vs);
        AddParameter(2, "Else", NodeBehTypePS, vs);
        result = new(false, BoolTypePS, this);
    }

   

   
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                foreach (var node in nodes)
                {
                    yield return StartCoroutine(node.ActivatorStart());
                }
                break;
            case TaskResult.PROCESS:
                yield return WaitForEndTask(OnUpdate);
                myTree.be.StartCoroutine(ActivatorStart());
                break;
            case TaskResult.ERROR:
                NodeBeh els = GetParameter<NodeBeh>("Else");

                if (els != null)
                {
                    yield return els.ActivatorStart();

                }
                else
                {
                    foreach (var node in nodes)
                    {
                        yield return StartCoroutine(node.ActivatorStart());
                    }
                    break;
                }
                break;
        }
    }

    public override bool Check()
    {
        bool r = InterGetParameter<string>("A") == InterGetParameter<string>("B");
        result.SetValue(r);
       return r;
    }
}
