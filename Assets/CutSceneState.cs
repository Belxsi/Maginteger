using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CutSceneState : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool active;
    public static TargetCutScene currentTarget;
   public static void StartCutScene(TargetCutScene target)
    {
        if (!active)
        {
            if (Player.TryGetPlayer())
            {
                currentTarget = target;
                Player.me.pc.enabled = false;
                LevelColliders.SetLevel(target.level);
              //  AutoWalk.StartMoveOfCutScene(target);

                active = true;
            }
        }
    }
    public void Update()
    {
        if(ScreenBlackCutScene.me!=null)
        ScreenBlackCutScene.me.SetBlack(active);
        if (active)
            if (Player.TryGetPlayer())
                if (currentTarget != null)
            {
                if (currentTarget.good)
                {
                    Player.me.pc.enabled = true;
                    
                    currentTarget = null;
                    active = false;
                }
            }
    }
}
[Serializable]
public class TargetCutScene
{
    public Vector2 pos;
    public bool good;
    public int level;
    public TargetCutScene(Vector2 pos,int level)
    {
        this.pos = pos;
        this.level = level;
    }
}
