using UnityEngine;

public class WalkForNPC : NodeBeh
{
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Velocity", Vector2TypePS, vs);
        AddParameter(1, "Target", Vector2TypePS, vs);
        AddParameter(2, "NPC",NPCTypePS, vs);
        AddParameter(3, "Simple", BoolTypePS, vs);
    }

    public override void OnStart()
    {
        NPCBehaviour npc = GetParameter<NPCBehaviour>("NPC");
        Vector2 vec = InterGetParameter<Vector2>("Velocity");
        if (!InterGetParameter<bool>("Simple"))
        {
            
            npc.WalkTo(vec, InterGetParameter<Vector2>("Target"));
        }
        else
        {
            npc.Walk(vec);
        }
        //SetParameter("Velocity", Vector2.zero, Vector2TypePS);
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
