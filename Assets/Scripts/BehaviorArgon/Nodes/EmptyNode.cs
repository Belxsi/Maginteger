using UnityEngine;

public class EmptyNode : NodeBeh
{
    public override void Init(params object[] vs)
    {
      
    }

    public override void OnStart()
    {
      
    }

    public override void OnUpdate()
    {
      
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
