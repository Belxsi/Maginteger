using UnityEngine;

public class MasterBehaviourNPC : NPCBehaviour
{
    public BehaviorExecutor behaviorExecutor;
    public void Awake()
    {
  
        
        
    }

    // Update is called once per frame
    void Update()
    {
        d.position = down;
        r.position = rigth;
        u.position = up;
        l.position = left;
        
    }
}
