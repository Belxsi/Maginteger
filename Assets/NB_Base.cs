using System.Collections.Generic;
using UnityEngine;

public class NB_Base : NPCBehaviour
{
    public bool active;
    
    public void Awake()
    {
        Init(this);
        Spells = new();
        Spells.Add(SpellsBase[Random.Range(0, SpellsBase.Count - 1)]);
        GetWallOffsets();
    }
    public static List<string> SpellsBase = new()
    {
        "MIE O V(1) R(1) E(10)",
        "MIE O V(0.5) R(2) E(10)",
        "MIE O V(0.25) R(1) E(5)",
        "MIE O V(3) R(0.3) E(10)",
        "MIE O V(0.1) R(0.1) E(2)",
        "MIE O V(5) R(2) E(15)",
        "MIE O V(1+[TIME]) R(1) E(10)",
        "MIE O V(1+[TIME]) R(1+[TIME]) E(10)",
        "MIE O V(1) R(1+[TIME]) E(10)",
        "MIE O V(1+[TIME]) R(1+[TIME]) E(10)",
        "MIE O V(1) R(1) E(25)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+1/A) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+1/A) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1) R(1+1/A) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+1/A) R(1+1/A) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+SIN(A)) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]*10) V(1+SIN(A)) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1) R(1+SIN(A)) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+SIN(A)) R(1+SIN(A)) E(10)",
        "MIE O VAR(A) FORM(A=[TIME]) V(1+1/SIN(A)) R(1) E(10)",
        "MIE O VAR(A) FORM(A=[MOUSEDIR]) V(1) R(1) E(10) DIR(-A)",
        "MIE O VAR(A,B) FORM(A=[PLAYERPOS]-[MEPOS]) FORM(B=[TIME]*10) V(1) R(1) E(10) DIR(A*SIN(B))",

    };
    public List<string> Spells = new();
    public float timeout = 0, time = 3, height = 10, offset = 0.5f;
    public Parameters parameters;
    public bool ready;
    public GameObject fire;

    public void Update()
    {
        if (!BaseFunc.Pause)
        {
            GetWallOffsets();
            d.position = down;
            r.position = rigth;
            u.position = up;
            l.position = left;
            if (autoWalk.tcs == null)
            {

                if (Player.TryGetPlayer())
                {
                 //   if (Player.me.NearEnergiesDirAndPos(transform.position, 14f, out Vector2 dir, out Vector2 near, out Energy energy))
                   // {
                  //      Avoid(near, dir, energy);
                 //   }
                  //  else
                  //  {

                    //    Gauside();
//
                   // }
                }
                else
                {
                    Gauside();
                }

            }
            if (active)
                if (npc.cm.mattery == null)
                {
                    if (!ready)
                    {

                        npc.cm.FomritMagic(Spells[Random.Range(0, Spells.Count)], npc, npc.cm);
                        //time * Mathf.Abs(Mathf.Sin(Time.time) *
                        time = Mathf.PerlinNoise(time + 1, time + 1) * height;
                        timeout = Random.Range(0, time) + offset;
                        ready = true;
                        npc.npcAE.AudioAwake(npc.npcAE.concentrat, false);



                    }

                }

            if (ready)
            {
                if (Player.TryGetPlayer())
                    if (npc.cm.mattery.magic.spellActivator.TryGetParameter("MI", out Param mi))
                        if ((timeout <= 0) || (npc.GetDolyEnergy() - (1 - Magic.GetCircleStrong((TypeMagicCircle)mi.value)) < 0.1f))
                        {
                            ready = false;

                            npc.cm.mattery.magic.dir = (Player.me.transform.position - transform.position).normalized;
                            npc.cm.Fire(npc.cm);
                            npc.npcAE.AudioStop(npc.npcAE.concentrat);
                            npc.npcAE.AudioAwake(npc.npcAE.fire, false, true);
                            npc.cm.mattery = null;
                        }

            }
            if (timeout > 0) timeout -= Time.deltaTime;


        }
    }




}
public class NP_BaseMapBehaviour
{

}
public class MapActor
{

}
