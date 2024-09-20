using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTrigger : MonoBehaviour
{
    public Transform PointTarget;
    public int level;
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name=="Player")
        CutSceneState.StartCutScene(new TargetCutScene(PointTarget.position,level));
    }

   
}
