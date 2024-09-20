using System.Collections;
using UnityEngine;

public class ParallelSequance : NodeBeh
{
    public override void Init(params object[] vs)
    {
        return;
    }

    public override void OnStart()
    {
        return;
    }

    public override void OnUpdate()
    {
        return;
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    public override IEnumerator ActivatorStart()
    {
        
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                foreach (var node in nodes)
                {
                    StartCoroutine(node.ActivatorStart());
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
}
