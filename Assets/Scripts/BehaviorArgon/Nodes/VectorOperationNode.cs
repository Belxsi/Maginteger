using UnityEngine;

public class VectorOperationNode : NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "A", FloatTypePS, vs);
        AddParameter(1, "B", FloatTypePS, vs);
        AddParameter(2, "Type", new() { typeof(TypeOperate) }, vs);

        result = new(0, FloatTypePS, this);
    }

    public override void OnStart()
    {
        Vector2 a = InterGetParameter<Vector2>("A"), b = InterGetParameter<Vector2>("B");
        switch (InterGetParameter<TypeOperate>("Type"))
        {
            case TypeOperate.Sum:
                result.SetValue(a + b);
                break;
            case TypeOperate.Sub:
                result.SetValue(a - b);
                break;
            case TypeOperate.Div:
                result.SetValue(a / b);
                break;
            case TypeOperate.Multi:
                result.SetValue(a * b);
                break;
        }
    }

    public override void OnUpdate()
    {
       
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
