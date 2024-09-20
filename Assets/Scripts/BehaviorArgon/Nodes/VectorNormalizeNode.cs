using UnityEngine;

public class VectorNormalizeNode:NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A", Vector2TypePS, vs);
      
        result = new(Vector2.zero, Vector2TypePS, this);
    }

    public override void OnStart()
    {
        result.SetValue(InterGetParameter<Vector2>("A").normalized);
    }

    public override void OnUpdate()
    {
        return;
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
