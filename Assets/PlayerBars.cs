using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerBars : MonoBehaviour
{
    public Scrollbar hit, energy,minEnergy;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Player.TryGetPlayer()){
            hit.size = Player.me.GetDolyLife();
            energy.size = Player.me.GetDolyEnergy();
            if (Player.me.cm.mattery != null)
                minEnergy.size = Player.me.cm.GetDolyMany();
        }
    }
}
