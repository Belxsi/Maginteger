using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCBars : MonoBehaviour
{
    public Scrollbar hit, energy, minEnergy;
    public NPC me;
    public void Init(NPC me)
    {
        this.me = me;
        Update();
    }


    public void Update()
    {
        
        if (me == null)
        {
            Destroy(gameObject);
        }else
        if (CanvasNPC.me != null)
        {
            Vector2 posWorld = me.transform.position;
            Vector2 posScreen = Camera.main.WorldToScreenPoint(posWorld);
            transform.SetParent(CanvasNPC.me.transform);
            transform.position = posScreen;
            hit.size = me.GetDolyLife();
            energy.size = me.GetDolyEnergy();
            if (me.cm.mattery != null)
                minEnergy.size = me.cm.GetDolyMany();
        }
    }
}
