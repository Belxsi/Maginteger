using System.Collections;
using UnityEngine;

public class IfNode : Condition
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Value", NodeBehTypePS, vs);
        AddParameter(1, "Else", NodeBehTypePS, vs);
        result = new(false,BoolTypePS, this);

    }
    public override bool Check()
    {
        bool r = InterGetParameter<bool>("Value");
        result.SetValue(r);
        return r;
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
                break;
        }
    }
}
