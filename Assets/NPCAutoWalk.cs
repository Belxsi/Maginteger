using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAutoWalk : MonoBehaviour
{
    public TargetAutoWalk tcs;
    public bool active;
   
    public NavMeshAgent agent;
    public NPCBehaviour behaviour;
    public TypeMove typeMove;
    void Awake()
    {
        if (typeMove == TypeMove.Navigate)
        {
          
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.updatePosition = false;
        }
        else
        {
            
        }

    }
   
    // Update is called once per frame
    void Update()
    {
        if (typeMove == TypeMove.Navigate)
        {
            agent.enabled = active;
            agent.speed = behaviour.npc.life.valueSpeed;
            agent.acceleration = behaviour.npc.life.valueSpeed;
            if ((active) & (tcs != null))
            {
                agent.SetDestination(tcs.pos);
                agent.updatePosition = true;
                float dist = Vector2.Distance(tcs.pos, agent.transform.position);
                // Player.me.GetMoveCharacter(BaseFunc.RoundVector( agent.velocity.normalized));
                behaviour.vectorMovement = agent.desiredVelocity.normalized;
                if ((dist < 0.5f) || (behaviour.vectorMovement == Vector2.zero))
                {
                    //active = false;

                    tcs = null;
                    agent.updatePosition = false;
                }

            }
        }
        else
        {
            if (tcs != null)
            {
                
                behaviour.npc.physic.Move(behaviour.npc.life.valueSpeed * (tcs.pos-(Vector2)transform.position).normalized);
               
            }
        }

    }
}
public class TargetAutoWalk
{
    public Vector2 pos;

    public TargetAutoWalk(Vector2 pos)
    {
        this.pos = pos;
    }
}
public enum TypeMove
{
    Navigate,
    Velocity
}