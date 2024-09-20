using UnityEngine;

public class SumVector:NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A", Vector2TypePS, vs);
        AddParameter(1, "B", Vector2TypePS, vs);
        result = new(Vector2.zero, Vector2TypePS, this);
    }

    public override void OnStart()
    {
        result.SetValue(InterGetParameter<Vector2>("A")+ InterGetParameter<Vector2>("B"));
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
