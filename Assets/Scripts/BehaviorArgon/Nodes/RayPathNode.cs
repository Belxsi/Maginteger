using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPathNode : NodeBeh
{
    public NodeParameter dir,pos,dist,type;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Dir", Vector2TypePS,vs);
        AddParameter(1, "Pos", Vector2TypePS, vs);
        AddParameter(2, "Count", IntTypePS, vs);
        AddParameter(3, "Exep", new() {typeof(List<string>) }, vs);
        AddParameter(4, "NPC", NPCTypePS, vs);
        dir = new(Vector2.zero,Vector2TypePS, this);
        pos = new(Vector2.zero, Vector2TypePS, this);
        dist = new(0f,FloatTypePS, this);
        type = new("", StringTypePS, this);

    }

    public override void OnStart()
    {
        NPCBehaviour npc = InterGetParameter<NPCBehaviour>("NPC");
        Ray2D ray = new(InterGetParameter<Vector2>("Pos"), InterGetParameter<Vector2>("Dir"));
        Vector2 point  = npc.LongestPath(ray, InterGetParameter<Vector2>("Pos"), InterGetParameter<List<string>>("Exep"),out RaycastHit2D hit);
        if(hit.collider.TryGetComponent<Energy>(out Energy elem))
        {
            type.SetValue("Energy");
        }
        if (hit.collider.TryGetComponent<LinkForParent>(out LinkForParent LIN))
        {
            type.SetValue("Player");
        }
        Vector2 dird = point - (Vector2)transform.position;
        pos.SetValue(point);
        dist.SetValue(Vector2.Distance(point, transform.position));
        dir.SetValue(dird.normalized );

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
