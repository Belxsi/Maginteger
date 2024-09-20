using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class TimeOutNode : NodeBeh
{

    public float timeout;
    public override void Init(params object[] vs)
    {

        AddParameter(0, "Time",FloatTypePS ,vs);
        
    }

    public override void OnStart()
    {
        timeout = Convert.ToSingle(GetParameter("Time",null).GetValue());
    }

    public override void OnUpdate()
    {
        timeout -= Time.deltaTime;
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                foreach (var node in nodes)
                {
                    yield return node.ActivatorStart();
                }
                break;
            case TaskResult.PROCESS:
                yield return WaitForEndTask(OnUpdate);
                foreach (var node in nodes)
                {
                    yield return node.ActivatorStart();
                }
                break;
            case TaskResult.ERROR:

                break;
        }
    }
    public override TaskResult TaskUpdate()
    {
        if (timeout > 0)
        {
            return TaskResult.PROCESS;
        }
        else
            return TaskResult.COMPLETE;
    }
}
