using UnityEngine;

public class SpellCasterNode : NodeBeh
{
    public NodeParameter nocasted;
    public override void Init(params object[] vs)
    {
        AddParameter(0, "NPC", NPCTypePS, vs);
        AddParameter(1, "Spell", StringTypePS, vs);
        AddParameter(2, "Min", FloatTypePS, vs);
        AddParameter(3, "Dir", Vector2TypePS, vs);
        nocasted = new(true, BoolTypePS, this);

    }
    public bool ready;
    public override void OnStart()
    {
        float min = InterGetParameter<float>("Min");
        NPC npc = InterGetParameter<NPCBehaviour>("NPC").npc;
        string spell = InterGetParameter<string>("Spell");
        npc.cm.creater = npc;
        if (npc.cm.mattery == null)
            {
            if (!ready)
            {
              
                    
               
                    if (npc.cm.CloseEnergyToLimit(0.413496671566344f) > 0.1f)

                    {
                        nocasted.SetValue(false);

                        //time * Mathf.Abs(Mathf.Sin(Time.time) *
                        npc.cm.FomritMagic(spell, npc, npc.cm);
                        ready = true;
                    npc.npcAE.AudioAwake(npc.npcAE.concentrat, false);
                      


                    }
                
            }

            }
        if (npc.cm.CloseEnergyToLimit() <= 0f)
        {
            ready = false;
        }
        else
        {
            if (npc.cm.CloseEnergyToLimit()<Mathf.Clamp01(min))
            {
                
                    if (Player.TryGetPlayer())
                        if (npc.cm.mattery.magic.spellActivator.TryGetParameter("MI", out Param mi))
                        {
                            ready = false;

                            npc.cm.mattery.magic.dir = InterGetParameter<Vector2>("Dir");
                            npc.cm.Fire(npc.cm);
                            npc.npcAE.AudioStop(npc.npcAE.concentrat);
                            npc.npcAE.AudioAwake(npc.npcAE.fire, false, true);
                            npc.cm.mattery = null;
                        }

                
            }
        }
        
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
