using UnityEngine;

public class GeneratorNPC 
{
    public static bool IsInit;
    public static GameObject NPC_prefab;
    public static void Init()
    {
        if (!IsInit)
        {
            IsInit = true;
            NPC_prefab = BaseFunc.GetPrefab("NPC");
        }
        
    }
    public static NPC SimpleNPCCreate(Vector3 pos,Quaternion rot,Transform parent)
    {
        Init();
        
        NPC npc = MonoBehaviour.Instantiate(NPC_prefab, pos, rot, parent).GetComponent<NPC>();
        return npc;
    }
}
