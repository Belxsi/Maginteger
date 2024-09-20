using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoWalk : MonoBehaviour
{
    public TargetCutScene tcs;
    public bool active;
   /*
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
       
    }
    public static void StartMoveOfCutScene(TargetCutScene target)
    {
        if (Player.TryGetPlayer())
        {
            AutoWalk autoWalk = Player.me.autoWalk;
            autoWalk.tcs = target;
            autoWalk.active = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        agent.enabled=active;
        if ((active)&(tcs!=null))
        {
            agent.SetDestination(tcs.pos);
            agent.updatePosition = true;
            float dist = Vector2.Distance(tcs.pos, agent.transform.position);
            // Player.me.GetMoveCharacter(BaseFunc.RoundVector( agent.velocity.normalized));
            if(Player.TryGetPlayer())
            Player.me.pc.vectorMovement =agent.desiredVelocity.normalized;
            if (dist <0.5f)
            {
                active = false;
                tcs.good = true;
                tcs = null;
                agent.updatePosition = false;
            }
            
        }
    }
   */
}
