using UnityEngine;

public class VectorPerlinRandom : NodeBeh
{
    public NodeParameter result;
    public override void Init(params object[] vs)
    {
        result = new(Vector2.zero,Vector2TypePS, this);
        return;
    }
    public float t=0f;
    public Vector2 res;
    public override void OnStart()
    {

        t += UnityEngine.Random.Range(-1f, 1f);
        res = Quaternion.Euler(0, 0, t) * res;
        result.SetValue(res.normalized);
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
