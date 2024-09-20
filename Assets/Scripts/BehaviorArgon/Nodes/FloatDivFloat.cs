using UnityEngine;

public class FloatDivFloat:NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A",FloatTypePS, vs);
        AddParameter(1, "B", FloatTypePS, vs);
        result = new(0f, FloatTypePS, this);
    }

    public override void OnStart()
    {
        result.SetValue(InterGetParameter<float>("A") / InterGetParameter<float>("B"));
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
