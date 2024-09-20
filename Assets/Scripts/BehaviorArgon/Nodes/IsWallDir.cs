using System.Collections;
using UnityEngine;

public class IsWallDir : Condition
{
    public NodeParameter PointWall;
    public bool Raycast(Vector2 vector2,float distance, out Vector2 point)
    {
        point = Vector2.zero;
        RaycastHit2D[] hit = new RaycastHit2D[1];
        ContactFilter2D contactFilter2D = new();
        contactFilter2D.maxDepth = 25;
        contactFilter2D.layerMask = new();
        LayerMask layer = new();
        layer.value = 1 << 9;
        contactFilter2D.SetLayerMask(layer);

        Physics2D.Raycast(transform.position, vector2, contactFilter2D, hit, distance);

        if (hit[^1].collider != null)
        {
            if (vector2 == Vector2.up)
            {

            }
            point = hit[^1].point;
            Debug.DrawLine(transform.position, point + vector2, Color.red);
            return true;
        }
        return false;
    }
    public override bool Check()
    {
        bool result = Raycast(InterGetParameter<Vector2>("Dir"),InterGetParameter<float>("Dist"), out Vector2 point);
        PointWall.SetValue(point);
        return result;
    }

    public override void Init(params object[] vs)
    {
        AddParameter(0, "Dir", Vector2TypePS, vs);
        AddParameter(1, "Dist",FloatTypePS, vs);
        AddParameter(2, "Else",NodeBehTypePS, vs);
        PointWall = new(Vector2.zero, Vector2TypePS, this);
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


}
