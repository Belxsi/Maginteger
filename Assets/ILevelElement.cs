using System.Collections.Generic;
using UnityEngine;

public abstract class ILevelElement : MonoBehaviour
{
    public MapChunck chunck;
    public GameObject grid;
    public GameObject[] SubVariants;
    public List<GlobalDistInvisibility> Special=new();
    public GameObject CurrentVariant;
    public RoomFight fight;

    public List<NPC> InstaningNPC()
    {
        List<NPC> npcs = new();
        for (int i = 0; i < Random.Range(1, 5); i++)
        {
            NPC npc = GeneratorNPC.SimpleNPCCreate(transform.position + (Vector3)Random.insideUnitCircle * 5, Quaternion.identity, GameplayPublicField.BigFather()).GetComponent<NPC>();
            npc.fight = fight;
            npcs.Add(npc);
        }
        return npcs;


    }
    public List<NPC> FinalInstaningNPC()
    {
        List<NPC> npcs = new();
        for (int i = 0; i < 10; i++)
        {
            NPC npc = GeneratorNPC.SimpleNPCCreate(transform.position + (Vector3)Random.insideUnitCircle * 5, Quaternion.identity, GameplayPublicField.BigFather()).GetComponent<NPC>();
            npc.fight = fight;
            npcs.Add(npc);
        }
        return npcs;


    }
    public void Init()
    {
        switch (chunck.type)
        {
            case "room":
                switch (chunck.state)
                {
                    case "verify":
                        fight.npcs = InstaningNPC();

                        break;

                    case "spawn":

                        fight.state = RoomFight.StateRoomFight.Sleep;
                        break;
                    case "final":
                        Instantiate(BaseFunc.GetPrefab("Win"), transform.position, Quaternion.identity, GameplayPublicField.BigFather());
                        name = "Final";
                        fight.npcs = FinalInstaningNPC();
                        break;
                }
                break;
        }
    }
    
    public void SetInvisSpecial(float intensive,bool control)
    {
        foreach(GlobalDistInvisibility item in Special)
        {
            item.control = control;
            StartCoroutine(item.SetInvisibility(intensive));
        }
    }
    public void SetCurrentSubVariant()
    {
        GameObject[] SubVariants = chunck.variant.prefab.GetComponent<Room>().SubVariants;
        switch (chunck.udlr.x + "" + chunck.udlr.y + "" + chunck.udlr.z + "" + chunck.udlr.w + "")
        {
            case "1000":
                CurrentVariant = SubVariants[0];
                break;
            case "0100":
                CurrentVariant = SubVariants[1];
                break;
            case "0010":
                CurrentVariant = SubVariants[2];
                break;
            case "0001":
                CurrentVariant = SubVariants[3];
                break;
            case "1001":
                CurrentVariant = SubVariants[4];
                break;
            case "0101":
                CurrentVariant = SubVariants[5];
                break;
            case "0110":
                CurrentVariant = SubVariants[6];
                break;
            case "1010":
                CurrentVariant = SubVariants[7];
                break;
            case "1100":
                CurrentVariant = SubVariants[8];
                break;
            case "0011":
                CurrentVariant = SubVariants[9];
                break;
            case "1111":
                CurrentVariant = SubVariants[10];
                break;
            case "0000":
                CurrentVariant = SubVariants[11];
                break;
            case "0111":
                CurrentVariant = SubVariants[12];
                break;
            case "1011":
                CurrentVariant = SubVariants[13];
                break;
            case "1101":
                CurrentVariant = SubVariants[14];
                break;
            case "1110":
                CurrentVariant = SubVariants[15];
                break;
            default:
                
                break;

        }
        CurrentVariant = Instantiate(CurrentVariant, grid.transform);
        if (SubVariants.Length > 16)
        {
            for(int i=16;i< SubVariants.Length; i++)
            {
                GameObject spec = Instantiate(SubVariants[i], grid.transform);
                spec.SetActive(true);
               Special.Add(spec.GetComponent<GlobalDistInvisibility>());

            }
        }
        CurrentVariant.SetActive(true);
    }
    public void SetChunck(MapChunck chunck)
    {
        this.chunck = chunck;
    }
    public MapChunck GetChunck()
    {
        return chunck;
    }

}
