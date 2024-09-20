using UnityEngine;

public abstract class Condition : NodeBeh
{
    public override void Init(params object[] vs)
    {
        return;
    }

    public override void OnStart()
    {
        return;
    }
    public abstract bool Check();

    public override void OnUpdate()
    {
        return;
    }

    public override TaskResult TaskUpdate()
    {
        if (Check())
        {
            return TaskResult.COMPLETE;
        }
        else
        {
            return TaskResult.ERROR;
        }
    }
}
