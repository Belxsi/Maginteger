using System.Collections;
using UnityEngine;

public class AvoidingWall : NodeBeh
{
    public NodeParameter neared, dist;

    public override void Init(params object[] vs)
    {
        AddParameter(0, "NPC", NPCTypePS, vs);
        neared = new(Vector2.zero, Vector2TypePS, this);
        dist = new(0f, FloatTypePS, this);
    }

    public override void OnStart()
    {
        
        NPCBehaviour npc = InterGetParameter<NPCBehaviour>("NPC");
        Vector2 nearvec=Vector2.zero;
        float d=0f;
        npc.GetWallOffsets();
        nearvec= npc.NearWall(out d);
        nearvec = ((Vector2)transform.position - nearvec).normalized;
        neared.SetValue(nearvec);
        dist.SetValue(d);

    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
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
    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
}
