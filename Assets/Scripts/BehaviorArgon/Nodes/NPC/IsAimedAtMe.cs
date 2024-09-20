using System;
using System.Collections;
using UnityEngine;

public class IsAimedAtMe : Condition
{
    public NodeParameter distance;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Pos",Vector2TypePS ,vs);
        AddParameter(1, "Dir", Vector2TypePS, vs);
        AddParameter(2, "Scale",FloatTypePS, vs);
        AddParameter(3, "MaxK",FloatTypePS, vs);
        AddParameter(4, "Else", NodeBehTypePS, vs);
        distance = new(0f,FloatTypePS, this);

    }
    public float MiddleSquaded(params float[] vs)
    {
        float sum=0;
        foreach(var v in vs)
        {
            sum += v * v;
        }
        sum /= vs.Length;
        return Mathf.Sqrt(sum);
    }
    public float MiddleArefmetic(params float[] vs)
    {
        float sum = 0;
        foreach (var v in vs)
        {
            sum += v;
        }
        sum /= vs.Length;
        return sum;
    }
    public float KoffEquality(Vector2 a,Vector2 b)
    {
        a.Normalize();
        b.Normalize();
        float nx = (Mathf.Min(a.x, b.x)+1)/2, xx = (Mathf.Max(a.x, b.x)+1)/2;
        float ny = (Mathf.Min(a.y, b.y)+1)/2, xy = (Mathf.Max(a.y, b.y)+1)/2;
        float kx = Mathf.Abs( nx / xx ) ;
        float ky = Mathf.Abs(ny / xy) ;
        
        float result = MiddleSquaded(kx,ky) * 100;
        result = Mathf.Clamp(result, 0, 100f);
        if (float.IsInfinity(result)) result = 0;
        return result;
    }
    public override IEnumerator ActivatorStart()
    {
        OnStart();
        switch (TaskUpdate())
        {

            case TaskResult.COMPLETE:
                foreach (var node in nodes)
                {
                    yield return StartCoroutine(node.ActivatorStart());
                }
                break;
            case TaskResult.PROCESS:
                yield return WaitForEndTask(OnUpdate);
                myTree.be.StartCoroutine(ActivatorStart());
                break;
            case TaskResult.ERROR:
                NodeBeh els = GetParameter<NodeBeh>("Else");

                if (els != null)
                {
                    yield return els.ActivatorStart();

                }
                break;
        }
    }
    public override bool Check()
    {
        Vector2 pos = InterGetParameter<Vector2>("Pos"),
            dir= InterGetParameter<Vector2>("Dir");
        float MaxK = GetParameter<float>("MaxK");
        float sc = InterGetParameter<float>("Scale");
        Vector2 basedir = ((Vector2)transform.position - pos).normalized;
        float k=KoffEquality(basedir, dir);
        SetParameter("Koff", k,FloatTypePS);
        distance.SetValue(Vector2.Distance(pos, transform.position));
        return k*sc >= MaxK;


    }
}
