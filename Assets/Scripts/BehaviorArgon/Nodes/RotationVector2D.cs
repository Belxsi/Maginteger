using UnityEngine;

public class RotationVector2D : NodeBeh
{
    public NodeParameter RotatedVector;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Vector", Vector2TypePS, vs);
        AddParameter(1, "Angle",FloatTypePS, vs);
        RotatedVector = new(Vector2.zero, Vector2TypePS,this);
    }
    public static Vector2 RotateZ(Vector2 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        Vector2 result;
        float tx = v.x;
        float ty = v.y;
        result.x = (cos * tx) - (sin * ty);
        result.y = (cos * ty) + (sin * tx);
        return result;
    }
    public override void OnStart()
    {
        Vector2 vector = InterGetParameter<Vector2>("Vector");
        RotatedVector.SetValue(RotateZ(vector, GetParameter<float>("Angle")));

    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
