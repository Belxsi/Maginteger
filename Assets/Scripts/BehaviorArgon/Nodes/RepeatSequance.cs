using System.Collections;
using UnityEngine;

public class RepeatSequance : NodeBeh
{
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Infinity",BoolTypePS, vs);
        AddParameter(1, "Count",IntTypePS, vs);
    }
    public bool infinity;
    public int count;
    public override void OnStart()
    {
        infinity = GetParameter<bool>("Infinity");
        count = GetParameter<int>("Count");
    }

    public override void OnUpdate()
    {
        return;
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                if (infinity)
                {
                    while (infinity)
                    {

                        foreach (var node in nodes)
                        {
                            yield return StartCoroutine(node.ActivatorStart());
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < count; i++)
                    {
                        foreach (var node in nodes)
                        {
                            yield return StartCoroutine(node.ActivatorStart());
                        }
                    }
                }
                break;
            case TaskResult.PROCESS:
                yield return WaitForEndTask(OnUpdate);
                myTree.be.StartCoroutine(ActivatorStart());
                break;
            case TaskResult.ERROR:

                break;
        }
    }
    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
