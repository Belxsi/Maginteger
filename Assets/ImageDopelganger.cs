using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDopelganger : MonoBehaviour
{
    public List<GameObject> f = new();
    public NPC npc;

    
    void Update()
    {
        for (int i = 0; i < f.Count; i++) f[i].SetActive(false);
        switch (npc.IsMove)
        {
            case Dir.X:
                if (npc.playerVisual.spriteRenderer.flipX) { f[1].SetActive(true); }
                {
                    f[0].SetActive(true);
                }
                break;
            case Dir.B:
                f[2].SetActive(true);
                break;
            case Dir.F:
                f[3].SetActive(true);
                break;
        }
        
        
    }
}
